﻿using System;
using System.Collections.Generic;
using System.IO;
using Photon.Deterministic;
using Photon.Deterministic.Protocol;
using Photon.Deterministic.Server;

namespace Quantum
{
  public class CustomQuantumServer : DeterministicServer, IDisposable
  {
    DeterministicSessionConfig config;
    RuntimeConfig runtimeConfig;
    SessionContainer container;
    readonly Dictionary<String, String> photonConfig;
    RingBufferInputProvider inputProvider;

    public CustomQuantumServer(Dictionary<String, String> photonConfig) { this.photonConfig = photonConfig; }

    // here we're just caching the match configs (Deterministic and Runtime) for the authoritative simulation (match result validation)
    // optionally, we could modify the passed parameters so to have the configs defined on the server itself
    public override void OnDeterministicSessionConfig(DeterministicPluginClient client, SessionConfig configData)
    {
      config = configData.Config;
    }

    // same as comment on method above
    public override void OnDeterministicRuntimeConfig(DeterministicPluginClient client, Photon.Deterministic.Protocol.RuntimeConfig configData)
    {
      runtimeConfig = RuntimeConfig.FromByteArray(configData.Config);
    }

    private static ResourceManagerStaticPreloaded _resourceManager;
    private static QuantumJsonSerializer _serializer = new QuantumJsonSerializer();
    private static Object _lock = new Object();

    // Initializing the server simulation with required data
    public override void OnDeterministicStartSession()
    {
      lock (_lock)
      {
        // The FP math look up tables need to be loaded for the sim to run here
        if (!FPLut.IsLoaded)
        {
          String lutPath = Path.Combine(PluginLocation, photonConfig["PathToLUTFolder"]);
          PluginHost.LogInfo($"LUT path: {lutPath}");
          try {
            FPLut.Init(lutPath);
          } catch (Exception e) {
            PluginHost.LogError($"Failed to load LUT files from {lutPath}");
            PluginHost.LogException(e);
          }

          String pathToDB = Path.Combine(PluginLocation, photonConfig["PathToDBFile"]);
          PluginHost.LogInfo($"DB path: {pathToDB}");

          // Loading previously serialized data (quantum asset database and "replay" data - in this case because we need the SimulationConfig object from it)
          photonConfig.TryGetValue("EmbeddedDBFile", out String embeddedDBFilename);
          byte[] assetDBData = LoadAssetDBData(pathToDB, embeddedDBFilename ?? "Quantum.db.json");
          Assert.Always(assetDBData != null, "No asset database found");
          var assets = _serializer.DeserializeAssets(assetDBData);

          // since we're loading all the assets, need to setup allocator and utils now
          if (Native.Utils == null)
          {
            Native.Utils = SessionContainer.CreateNativeUtils();
          }
          
          // need to call Loaded on all the assets
          _resourceManager = ResourceManagerStaticPreloaded.Create(assets, SessionContainer.CreateNativeAllocator());
        }
      }

      // replacing the serialized configs the the match-specific one (in this case they came from the master-client)
      var configsFile = new ReplayFile();
      configsFile.DeterministicConfig = config;
      configsFile.RuntimeConfig = runtimeConfig;

      // a "standalone/non unity" session container is used for simulations (game loops).
      container = new SessionContainer(configsFile);
      // Container requires access to the configs (through a replay file), the assets database, and input (either through replay file or through direct injection - see below)

      var gameFlags = 0;

      // tag the server sim as Server (notably to receive game events that target the server and not receive events that target the client exclusively)
      gameFlags |= QuantumGameFlags.Server;

      // no need to allocate an extra frame for interpolation on the server sim (saves memory and frame copies)
      gameFlags |= QuantumGameFlags.DisableInterpolatableStates;

      var startParams = new QuantumGame.StartParameters {
        AssetSerializer = _serializer,
        ResourceManager = _resourceManager,
        GameFlags = gameFlags,
      };

      // calling Start() sets up everything in the container
      inputProvider = new RingBufferInputProvider(config);

      // force the server simulation to run in single thread (scales better)
      var taskRunner = new InactiveTaskRunner();
      
      container.StartReplay(startParams, inputProvider, "server", logInitForConsole: false, taskRunner: taskRunner);
    }

    // Every time the plugin confirms input, we inject the confirmed data into the container, so server simulation can advance
    public override void OnDeterministicInputConfirmed(DeterministicPluginClient client, int tick, int playerIndex, DeterministicTickInput input)
    {
      inputProvider.InjectInput(input, true);
    }

    // Called when input from one client-controlled player is accepted by the server (use this for authoritative input replacement)
    public override void OnDeterministicInputReceived(DeterministicPluginClient client, DeterministicTickInput input)
    {
    }

    // Called when server generates input for non-connected players (use this to generate AI input from server)
    public override bool OnDeterministicServerInput(DeterministicTickInput input)
    {
      // return true if input was generated from this method.
      return false;
    }

    // use this to intercept when players send RuntimePlayer data;
    public override bool OnDeterministicPlayerDataSet(DeterministicPluginClient client, SetPlayerData playerData)
    {
      // return false to reject the data (true to accept);
      return true;
    }

    public override void OnDeterministicUpdate()
    {
      if (container == null) {
        return;
      }
      
      try
      {
        // advance the simulation to the latest injected input-set/tick
        if (container.Session.FrameVerified != null)
        {
          // server plugin game time
          double gameTime = Session.Input.GameTime;
          
          // server simulation non synced timed
          var sessionTime = container.Session.AccumulatedTime + container.Session.FrameVerified.Number * container.Session.DeltaTimeDouble;
          
          // try to update session to catch-up (will still be bound by input)
          container.Service(gameTime - sessionTime);
        } 
        else
        {
          container.Service();
        }
      } catch (Exception e)
      {
        PluginHost.LogError($"An exception was thrown while servicing the SessionContainer (exception log follows). The container will be destroyed.");
        PluginHost.LogException(e);

        container?.Destroy();
        container = null;
      }
    }

    public override Boolean OnDeterministicSnapshotRequested(ref Int32 tick, ref byte[] data) {
      if (container == null || container.Session.FramePredicted == null) {
        // Too early for snapshots.
        return false;
      }

      tick = container.Session.FramePredicted.Number;
      data = container.Session.FramePredicted.Serialize(DeterministicFrameSerializeMode.Serialize);
      return true;
    }

    public string PluginLocation
    {
      get
      {
        string codeBase = GetType().Assembly.CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
      }
    }

    public void Dispose() {
      container?.Destroy();
      container = null;
    }

    // Loads the asset DB in json form from 1) disk file 2) the from embedded file
    private byte[] LoadAssetDBData(string pathToDB, string embeddedDB) 
    {
      byte[] assetDBFileContent = null;

      // Trying to load the asset db file from disk
      if (string.IsNullOrEmpty(pathToDB) == false) {
        if (File.Exists(pathToDB)) {
          PluginHost.LogInfo($"Loading Quantum AssetDB from file '{pathToDB}' ..");
          assetDBFileContent = File.ReadAllBytes(pathToDB);
          Assert.Always(assetDBFileContent != null);
        } else {
          PluginHost.LogInfo($"No asset db file found at '{pathToDB}'.");
        }
      }

      // Trying to load the asset db file from the assembly
      if (assetDBFileContent == null) {
        PluginHost.LogInfo($"Loading Quantum AssetDB from internal resource '{embeddedDB}'");
        using (var stream = typeof(QuantumGame).Assembly.GetManifestResourceStream(embeddedDB)) {
          if (stream != null) {
            if (stream.Length > 0) {
              assetDBFileContent = new byte[stream.Length];
              var bytesRead = stream.Read(assetDBFileContent, 0, (int)stream.Length);
              Assert.Always(bytesRead == (int)stream.Length);
            } else {
              PluginHost.LogError($"The file '{embeddedDB}' in assembly '{typeof(QuantumGame).Assembly.FullName}' is empty.");
            }
          } else {
            PluginHost.LogError($"Failed to find the Quantum AssetDB resource from '{embeddedDB}' in assembly '{typeof(QuantumGame).Assembly.FullName}'. Here are all resources found inside the assembly:");
            foreach (var name in typeof(QuantumGame).Assembly.GetManifestResourceNames()) {
              PluginHost.LogInfo(name);
            }
          }
        }
      }

      return assetDBFileContent;
    }
  }
}

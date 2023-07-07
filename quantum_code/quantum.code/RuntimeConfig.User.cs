using Photon.Deterministic;
using System;

namespace Quantum
{
    partial class RuntimeConfig
    {
        public AssetRefEntityPrototype CharacterPrototype;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref CharacterPrototype.Id);
        }
    }
}
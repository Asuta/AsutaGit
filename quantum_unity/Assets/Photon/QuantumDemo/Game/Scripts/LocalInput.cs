using System;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public class LocalInput : MonoBehaviour
{
    public Transform targetCube;

    //define a transform list
    public Transform[] targetCubes;
    public Transform[] playerOneTarget;
    public Transform[] playerTwoTarget;

    ////////////////test    ///////////////////////

    public int testInt = 0;
    public float testFloat = 0;
    public int outInt;

    void OnEnable()
    {
        QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));
    }

    public void PollInput(CallbackPollInput callback)
    {
        Quantum.Input i = new Quantum.Input();
        i.XX0 = (byte)MapTo255(playerOneTarget[0].rotation.x);
        i.YY0 = (byte)MapTo255(playerOneTarget[0].rotation.y);
        i.ZZ0 = (byte)MapTo255(playerOneTarget[0].rotation.z);
        i.WW0 = (byte)MapTo255(playerOneTarget[0].rotation.w);

        i.XX1 = (byte)MapTo255(playerOneTarget[1].rotation.x);
        i.YY1 = (byte)MapTo255(playerOneTarget[1].rotation.y);
        i.ZZ1 = (byte)MapTo255(playerOneTarget[1].rotation.z);
        i.WW1 = (byte)MapTo255(playerOneTarget[1].rotation.w);

        i.XX2 = (byte)MapTo255(playerOneTarget[2].rotation.x);
        i.YY2 = (byte)MapTo255(playerOneTarget[2].rotation.y);
        i.ZZ2 = (byte)MapTo255(playerOneTarget[2].rotation.z);
        i.WW2 = (byte)MapTo255(playerOneTarget[2].rotation.w);

        i.XX3 = (byte)MapTo255(playerOneTarget[3].rotation.x);
        i.YY3 = (byte)MapTo255(playerOneTarget[3].rotation.y);
        i.ZZ3 = (byte)MapTo255(playerOneTarget[3].rotation.z);
        i.WW3 = (byte)MapTo255(playerOneTarget[3].rotation.w);

        i.XX4 = (byte)MapTo255(playerOneTarget[4].rotation.x);
        i.YY4 = (byte)MapTo255(playerOneTarget[4].rotation.y);
        i.ZZ4 = (byte)MapTo255(playerOneTarget[4].rotation.z);
        i.WW4 = (byte)MapTo255(playerOneTarget[4].rotation.w);

        i.XX5 = (byte)MapTo255(playerOneTarget[5].rotation.x);
        i.YY5 = (byte)MapTo255(playerOneTarget[5].rotation.y);
        i.ZZ5 = (byte)MapTo255(playerOneTarget[5].rotation.z);
        i.WW5 = (byte)MapTo255(playerOneTarget[5].rotation.w);

        i.XX6 = (byte)MapTo255(playerOneTarget[6].rotation.x);
        i.YY6 = (byte)MapTo255(playerOneTarget[6].rotation.y);
        i.ZZ6 = (byte)MapTo255(playerOneTarget[6].rotation.z);
        i.WW6 = (byte)MapTo255(playerOneTarget[6].rotation.w);

        i.XX7 = (byte)MapTo255(playerOneTarget[7].rotation.x);
        i.YY7 = (byte)MapTo255(playerOneTarget[7].rotation.y);
        i.ZZ7 = (byte)MapTo255(playerOneTarget[7].rotation.z);
        i.WW7 = (byte)MapTo255(playerOneTarget[7].rotation.w);

        
        
        
        
        // i.TargetRotation = targetCube.rotation.ToFPQuaternion();
        // i.TargetRotationOne = targetCubes[0].rotation.ToFPQuaternion();
        // i.TargetRotationTwo = targetCubes[1].rotation.ToFPQuaternion();
        // i.TargetRotationThree = targetCubes[2].rotation.ToFPQuaternion();

        // i.BodyTargetRotationOne  = playerOneTarget[0].rotation.ToFPQuaternion();
        // i.BodyTargetRotationTwo  = playerOneTarget[1].rotation.ToFPQuaternion();
        // i.BodyTargetRotationThree  = playerOneTarget[2].rotation.ToFPQuaternion();
        // i.BodyTargetRotationFour  = playerOneTarget[3].rotation.ToFPQuaternion();
        // i.BodyTargetRotationFive  = playerOneTarget[4].rotation.ToFPQuaternion();
        // i.BodyTargetRotationSix  = playerOneTarget[5].rotation.ToFPQuaternion();
        // i.BodyTargetRotationSeven  = playerOneTarget[6].rotation.ToFPQuaternion();
        // i.BodyTargetRotationEight  = playerOneTarget[7].rotation.ToFPQuaternion();





        // var x = UnityEngine.Input.GetAxis("Horizontal");
        // var y = UnityEngine.Input.GetAxis("Vertical");

        //i.Direction = new Vector2(x, y).ToFPVector2();

        // using the property setter to convert from a vector to the internal byte
        //i.Direction = new FPVector2(x.ToFP(), y.ToFP());

        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }

    public static int MapTo255(float value)
    {
        // 将-1到1之间的数映射到0到255之间的整数
        int mappedValue = (int)((value + 1) * 127.5f);
        return mappedValue;
    }
}

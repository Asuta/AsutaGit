using Photon.Deterministic;
using System;

namespace Quantum
{
    partial struct Input
    {
        //public FPVector2 Direction
        //{
        //    get
        //    {
        //        if (XY == default)
        //            return default;

        //        Int32 angle = ((Int32)XY - 1) * 2;

                 

        //        return FPVector2.Rotate(FPVector2.Up, angle * FP.Deg2Rad);
        //    }
        //    set
        //    {
        //        if (value == default)
        //        {
        //            XY = default;
        //            return;
        //        }

        //        var angle = FPVector2.RadiansSigned(FPVector2.Up, value) * FP.Rad2Deg;

        //        angle = (((angle + 360) % 360) / 2) + 1;

        //        XY = (Byte)(angle.AsInt);
        //    }
        //}

        //public FPVector2 Direction1
        //{
        //    get
        //    {
        //        if (ZW == default)
        //            return default;

        //        Int32 angle = ((Int32)ZW - 1) * 2;



        //        return FPVector2.Rotate(FPVector2.Up, angle * FP.Deg2Rad);
        //    }
        //    set
        //    {
        //        if (value == default)
        //        {
        //            ZW = default;
        //            return;
        //        }

        //        var angle = FPVector2.RadiansSigned(FPVector2.Up, value) * FP.Rad2Deg;

        //        angle = (((angle + 360) % 360) / 2) + 1;

        //        ZW = (Byte)(angle.AsInt);
        //    }
        //}
    }
}
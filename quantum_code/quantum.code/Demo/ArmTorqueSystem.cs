using Photon.Deterministic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Quantum
{
    public unsafe class ArmTorqueSystem : SystemMainThreadFilter<ArmTorqueSystem.Filter>, ISignalOnPlayerDataSet
    {

        public int count = 0;
        public struct Filter
        {
            public EntityRef Entity;
            //public CharacterController3D* KCC;
            public Transform3D* Transform;
            //public PlayerLink* Link;
            public PhysicsBody3D* PhysicsBody;
            public ArmComponent* Arm;

        }

        public override void Update(Frame f, ref Filter filter)
        {
            count++;
            if (count >= 10000) count = 0;
            var input = f.GetPlayerInput(0);
            //var test = input->TargetRotation;
            var Arm = filter.Arm;
            var thisT = filter.Transform;

            FPQuaternion deltaRotation = new FPQuaternion(0, 0, 0, 0);



            
            








            // Log.Debug("Simulation xy   : " + input->Direction.ToString() );

            //creat a switch case ,and use the input->TargetRotationOne to control the arm(until to 7)
            switch (Arm->ArmNumber)
            {
                case 0:
                    var X = MapToMinusOneToOne(input->XX0);
                    var Y = MapToMinusOneToOne(input->YY0);
                    var Z = MapToMinusOneToOne(input->ZZ0);
                    var W = MapToMinusOneToOne(input->WW0);
                    FPQuaternion TargetRotationOne = new FPQuaternion(X, Y, Z, W);
                    deltaRotation = TargetRotationOne * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 1:
                    var X1 = MapToMinusOneToOne(input->XX1);
                    var Y1 = MapToMinusOneToOne(input->YY1);
                    var Z1 = MapToMinusOneToOne(input->ZZ1);
                    var W1 = MapToMinusOneToOne(input->WW1);
                    FPQuaternion TargetRotationTwo = new FPQuaternion(X1, Y1, Z1, W1);
                    //Log.Debug("1111111111" + TargetRotationTwo);
                    deltaRotation = TargetRotationTwo * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 2:
                    var X2 = MapToMinusOneToOne(input->XX2);
                    var Y2 = MapToMinusOneToOne(input->YY2);
                    var Z2 = MapToMinusOneToOne(input->ZZ2);
                    var W2 = MapToMinusOneToOne(input->WW2);
                    FPQuaternion TargetRotationThree = new FPQuaternion(X2, Y2, Z2, W2);
                    //Log.Debug("222222222" + TargetRotationThree);
                    deltaRotation = TargetRotationThree * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 3:
                    var X3 = MapToMinusOneToOne(input->XX3);
                    var Y3 = MapToMinusOneToOne(input->YY3);
                    var Z3 = MapToMinusOneToOne(input->ZZ3);
                    var W3 = MapToMinusOneToOne(input->WW3);
                    FPQuaternion TargetRotationFour = new FPQuaternion(X3, Y3, Z3, W3);
                    deltaRotation = TargetRotationFour * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 4:
                    var X4 = MapToMinusOneToOne(input->XX4);
                    var Y4 = MapToMinusOneToOne(input->YY4);
                    var Z4 = MapToMinusOneToOne(input->ZZ4);
                    var W4 = MapToMinusOneToOne(input->WW4);
                    FPQuaternion TargetRotationFive = new FPQuaternion(X4, Y4, Z4, W4);
                    deltaRotation = TargetRotationFive * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 5:
                    var X5 = MapToMinusOneToOne(input->XX5);
                    var Y5 = MapToMinusOneToOne(input->YY5);
                    var Z5 = MapToMinusOneToOne(input->ZZ5);
                    var W5 = MapToMinusOneToOne(input->WW5);
                    FPQuaternion TargetRotationSix = new FPQuaternion(X5, Y5, Z5, W5);
                    deltaRotation = TargetRotationSix * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 6:
                    var X6 = MapToMinusOneToOne(input->XX6);
                    var Y6 = MapToMinusOneToOne(input->YY6);
                    var Z6 = MapToMinusOneToOne(input->ZZ6);
                    var W6 = MapToMinusOneToOne(input->WW6);
                    FPQuaternion TargetRotationSeven = new FPQuaternion(X6, Y6, Z6, W6);
                    deltaRotation = TargetRotationSeven * FPQuaternion.Inverse(thisT->Rotation);
                    break;
                case 7:
                    var X7 = MapToMinusOneToOne(input->XX7);
                    var Y7 = MapToMinusOneToOne(input->YY7);
                    var Z7 = MapToMinusOneToOne(input->ZZ7);
                    var W7 = MapToMinusOneToOne(input->WW7);
                    FPQuaternion TargetRotationEight = new FPQuaternion(X7, Y7, Z7, W7);
                    //Log.Debug("Simulation xy   : " + input->XX7.ToString() + input->YY7.ToString() + input->ZZ7.ToString() + input->WW7.ToString());
                    deltaRotation = TargetRotationEight * FPQuaternion.Inverse(thisT->Rotation);
                    break;




            }





            //var input = f.GetPlayerInput(filter.Link->Player);



            //if (input->Jump.WasPressed)
            //{
            //    Debug.WriteLineIf(true, "Jump1");
            //    Debug.WriteIf(true, "Jump2");
            //    Debug.Assert(true, "Jump3");
            //    filter.PhysicsBody->AddLinearImpulse(new FPVector3(0, 100, 0));

            //}

            if (count % 3== 0)
            {
                FPVector3 torqueVector = new FPVector3(deltaRotation.X, deltaRotation.Y, deltaRotation.Z) * 30;
                // 施加扭矩
                filter.PhysicsBody->AddTorque(torqueVector);
            }
            //else if (count % 5 == 0)
            //{
            //    FPVector3 downForce = new FPVector3(0, -30, 0);
            //    filter.PhysicsBody->AddForce(downForce);
            //}

            ////FPQuaternion TargetRotation = new FPQuaternion(X, Y, Z, W);
            ////FPQuaternion deltaRotation = TargetRotation * FPQuaternion.Inverse(thisT->Rotation);
            ////// 根据差距计算出需要施加的扭矩
            //FPVector3 torqueVector = new FPVector3(deltaRotation.X, deltaRotation.Y, deltaRotation.Z) * 30;
            //// 施加扭矩
            //filter.PhysicsBody->AddTorque(torqueVector);
            ////FPVector3 downForce = new FPVector3(0, -10, 0);
            ////filter.PhysicsBody->AddForce(downForce);

            //switch (Arm->ArmNumber)
            //{
            //    case 1:
            //        //Log.Debug("1111111111Torqueeeeeeee" + torqueVector);
            //        break;
            //    case 2:
            //        Log.Debug("22222222222Torqueeeeeeee" + torqueVector);
            //        break;
            //}





        }
        public static FP  MapToMinusOneToOne(FP value)
        {
            // 将0到255的整数映射回-1到1之间的浮点数

            FP mappedValue = (value / FP.FromFloat_UNSAFE(127.5f)) - 1;
            //FP mappedValue = (value / 127) - 1;
            return mappedValue;
        }

        public void OnPlayerDataSet(Frame f, PlayerRef player)//当玩家数据被设置的时候，就会调用这个函数
        {
            //var data = f.GetPlayerData(player);//这个是找到玩家的数据

            //var prototype = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);//这个是找到一个实体的原型（玩家控制的那个小cube）
            //var e = f.Create(prototype);//这个是创建一个实体（玩家控制的那个小cube）

            //if (f.Unsafe.TryGetPointer<PlayerLink>(e, out var pl))
            //{
            //    pl->Player = player;
            //}

            //if (f.Unsafe.TryGetPointer<Transform3D>(e, out var t))
            //{
            //    t->Position.X = 0 + player;
            //}
        }


    }
}

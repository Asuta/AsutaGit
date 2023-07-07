using Photon.Deterministic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Quantum
{
    public unsafe class AddtorqueSystem : SystemMainThreadFilter<AddtorqueSystem.Filter>, ISignalOnPlayerDataSet
    {

        public struct Filter
        {
            public EntityRef Entity;
            //public CharacterController3D* KCC;
            public Transform3D* Transform;
            public PlayerLink* Link;
            public PhysicsBody3D* PhysicsBody;
            
        }

        public override void Update(Frame f, ref Filter filter)
        {
            //var input = f.GetPlayerInput(0);
            var input = f.GetPlayerInput(filter.Link->Player);

            var thisT = filter.Transform;

            //if(input->Jump.WasPressed)
            //{
            //    Debug.WriteLineIf(true, "Jump1");
            //    Debug.WriteIf(true, "Jump2");
            //    Debug.Assert(true, "Jump3");
            //    filter.PhysicsBody->AddLinearImpulse(new FPVector3(0, 100, 0));
                
            //}

            
            //FPQuaternion deltaRotation = input->TargetRotation * FPQuaternion.Inverse(thisT->Rotation);
            // 根据差距计算出需要施加的扭矩
            //FPVector3 torqueVector = new FPVector3(deltaRotation.X, deltaRotation.Y, deltaRotation.Z) * 30;
            // 施加扭矩
            
            
            
            //filter.PhysicsBody->AddTorque(torqueVector);
        }

        public void OnPlayerDataSet(Frame f, PlayerRef player)//当玩家数据被设置的时候，就会调用这个函数
        {
            var data = f.GetPlayerData(player);//这个是找到玩家的数据

            var prototype = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);//这个是找到一个实体的原型（玩家控制的那个小cube）
            var e = f.Create(prototype);//这个是创建一个实体（玩家控制的那个小cube）

            if (f.Unsafe.TryGetPointer<PlayerLink>(e, out var pl))
            {
                pl->Player = player;
            }

            if (f.Unsafe.TryGetPointer<Transform3D>(e, out var t))
            {
                t->Position.X = 0 + player;
            }
        }


    }
}

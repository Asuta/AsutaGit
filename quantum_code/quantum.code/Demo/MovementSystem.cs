using Photon.Deterministic;

namespace Quantum
{
    public unsafe class MovementSystem:SystemMainThreadFilter<MovementSystem.Filter>,ISignalOnPlayerDataSet
    {

        public struct Filter
        {
            public EntityRef Entity;
            public CharacterController3D* KCC;
            public Transform3D* Transform;
            public PlayerLink* Link;
            public PhysicsBody3D* PhysicsBody;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.Link->Player);

            //if(input ->Jump.WasPressed)
            //{
            //    filter.KCC->Jump(f);
            //}

            //speed hack protection
            //if(input->Direction.SqrMagnitude >1)
            //{
            //    input->Direction = input->Direction.Normalized;
            //}

            //filter.KCC->Move(f, filter.Entity, input->Direction.XOY);

            //if(input->Direction!= default)
            //{
            //    filter.Transform->Rotation = Photon.Deterministic.FPQuaternion.LookRotation(input->Direction.XOY);
            //}

            //filter.Transform->Position = new FPVector3(9, 4, 4);
            //filter.PhysicsBody->AddTorque(new FPVector3(1, 444, 5555));
        }

        public void OnPlayerDataSet(Frame f, PlayerRef player)//当玩家数据被设置的时候，就会调用这个函数
        {
            var data = f.GetPlayerData(player);//这个是找到玩家的数据

            var prototype = f.FindAsset<EntityPrototype>(data.CharacterPrototype.Id);//这个是找到一个实体的原型（玩家控制的那个小cube）
            var e = f.Create(prototype);//这个是创建一个实体（玩家控制的那个小cube）

            if(f.Unsafe.TryGetPointer<PlayerLink>(e,out var pl))
            {
                pl->Player = player;
            }

            if(f.Unsafe.TryGetPointer<Transform3D>(e,out var t))
            {
                t->Position.X = 0 + player;
            }
        }
    }
}

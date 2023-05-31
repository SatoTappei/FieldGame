using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct BulletMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // •ûŒü‚ÌComponent‚ª–³‚¢ê‡‚ÉOnUpdate()‚ªÀs‚³‚ê‚È‚¢‚æ‚¤‚É‚·‚é
        state.RequireForUpdate<BulletDirectionComponent>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        new BulletMoveJob { _deltaTime = deltaTime }.Schedule();
    }

    [BurstCompile]
    public partial struct BulletMoveJob : IJobEntity
    {
        public float _deltaTime;

        [BurstCompile]
        public void Execute(ref BulletMoveAspect aspect)
        {
            float3 dir = aspect._dir.ValueRO._value;
            float speed = aspect._speed.ValueRO._value;
            aspect._transform.TranslateWorld(dir * speed * _deltaTime);
        }
    }
}

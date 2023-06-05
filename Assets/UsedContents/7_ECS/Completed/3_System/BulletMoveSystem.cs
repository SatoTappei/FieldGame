using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// 発射された弾を指定の方向/速度で動かすSystem
/// </summary>
[BurstCompile]
[UpdateAfter(typeof(ShootTheBulletSystem))]
public partial struct BulletMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // Aspectを指定するとエラーになる
        state.RequireForUpdate<BulletDirectionComponent>();
        state.RequireForUpdate<BulletSpeedComponent>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 弾を移動させる
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        float deltaTime = SystemAPI.Time.DeltaTime;
        new BulletMoveJob
        {
            _ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            _deltaTime = deltaTime,
        }.ScheduleParallel();

        #region Job未使用Ver
        //float deltaTime = SystemAPI.Time.DeltaTime;

        //// 全ての発射された弾はBulletDirectionComponentを持っているのでこれで十分
        //foreach (var transform in SystemAPI.Query<RefRW<LocalToWorldTransform>>()
        //    .WithAll<BulletDirectionComponent>())
        //{
        //    transform.ValueRW.Value.Position += new float3(0, 0, 1) * deltaTime;
        //}
        #endregion
    }

    /// <summary>
    /// 並列で弾の移動を行うJob
    /// </summary>
    [BurstCompile]
    public partial struct BulletMoveJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter _ecb;
        public float _deltaTime;

        [BurstCompile]
        void Execute( BulletMoveAspect aspect, [EntityInQueryIndex] int sortKey)
        {
            float3 dir = aspect._dir.ValueRO._value;
            float speed = aspect._speed.ValueRO._value;
            aspect._transform.TranslateWorld(dir * speed * _deltaTime);
        }
    }
}

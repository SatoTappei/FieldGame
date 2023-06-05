using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// ���˂��ꂽ�e���w��̕���/���x�œ�����System
/// </summary>
[BurstCompile]
[UpdateAfter(typeof(ShootTheBulletSystem))]
public partial struct BulletMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // Aspect���w�肷��ƃG���[�ɂȂ�
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
        // �e���ړ�������
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        float deltaTime = SystemAPI.Time.DeltaTime;
        new BulletMoveJob
        {
            _ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            _deltaTime = deltaTime,
        }.ScheduleParallel();

        #region Job���g�pVer
        //float deltaTime = SystemAPI.Time.DeltaTime;

        //// �S�Ă̔��˂��ꂽ�e��BulletDirectionComponent�������Ă���̂ł���ŏ\��
        //foreach (var transform in SystemAPI.Query<RefRW<LocalToWorldTransform>>()
        //    .WithAll<BulletDirectionComponent>())
        //{
        //    transform.ValueRW.Value.Position += new float3(0, 0, 1) * deltaTime;
        //}
        #endregion
    }

    /// <summary>
    /// ����Œe�̈ړ����s��Job
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

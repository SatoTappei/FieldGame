using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// MonoBehaviour側の入力された値に応じて弾を発射するSystem
/// </summary>
[BurstCompile]
[UpdateAfter(typeof(TriggerByMonoSystem))]
public partial struct ShootTheBulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PlayerBulletSpawnComponent component = SystemAPI.GetSingleton<PlayerBulletSpawnComponent>();
        if (component._active)
        {
            // 弾生成
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            new ShootTheBulletJob
            {
                _ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                _pos = component._pos,
                _dir = component._dir,
            }.Run();

            // 弾生成データのリセット
            component._active = false;
            component._pos = float3.zero;
            component._dir = float3.zero;
            SystemAPI.SetSingleton(component);
        }

        #region Job未使用ver
        //foreach (var holder in SystemAPI.Query<RefRO<BulletHolderAspect>())
        //{
        //    Entity entity = state.EntityManager.Instantiate(holder.ValueRO._entity);
        //    state.EntityManager.SetComponentData(entity, new BulletSpeedComponent
        //    {
        //        _value = 5.0f
        //    });
        //    state.EntityManager.SetComponentData(entity, new BulletDirectionComponent
        //    {
        //        _value = new float3(0, 0, 1)
        //    });

        //    // TODO:ShootTriggerをオフにする処理が必要
        //    //state.EntityManager.SetComponentEnabled<ShootTrigger>(holder, false);
        //}
        #endregion
    }
}

/// <summary>
/// 弾を生成するJob
/// </summary>
[BurstCompile]
public partial struct ShootTheBulletJob : IJobEntity
{
    public EntityCommandBuffer _ecb;
    public float3 _pos;
    public float3 _dir;

    void Execute(BulletHolderComponent holder)
    {
        Entity entity = _ecb.Instantiate(holder._prototype);
        
        // SetComponentで追加しようとするとエラーが出る
        _ecb.AddComponent(entity, new BulletSpeedComponent { _value = 5.0f });
        _ecb.AddComponent(entity, new BulletDirectionComponent { _value = _dir });
    }

    /// <summary>
    /// 座標に応じた回転と大きさがデフォルトのTransformを取得する
    /// </summary>
    UniformScaleTransform GetTransform(float3 pos)
    {
        return new UniformScaleTransform
        {
            Position = pos,
            Rotation = quaternion.identity,
            Scale = 1.0f,
        };
    }
}
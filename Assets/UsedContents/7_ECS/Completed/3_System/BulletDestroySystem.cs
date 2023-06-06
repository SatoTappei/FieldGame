using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

/// <summary>
/// 速度が一定以下になった弾を破壊するSystem
/// 弾の生成及び移動のSystemを実行後、最後に実行する
/// </summary>
[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial struct BulletDestroySystem : ISystem
{
    EntityQuery _query;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // 弾の速度を読み取るために、速度Componentを持ったEntityへの処理をするクエリを作成
        NativeArray<ComponentType> components = new NativeArray<ComponentType>(1, Allocator.Temp);
        components[0] = ComponentType.ReadOnly<BulletSpeedComponent>();
        _query = state.GetEntityQuery(components);
        components.Dispose();

        state.RequireForUpdate<BulletSpeedComponent>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // 速度が一定以下になったEntityを破壊
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        NativeArray<Entity> entities = _query.ToEntityArray(Allocator.Temp);
        foreach(Entity entity in entities)
        {
            var component = state.EntityManager.GetComponentData<BulletSpeedComponent>(entity);
            if (component._value < 10.0f)
            {
                ecb.DestroyEntity(0, entity);
            }
        }

        entities.Dispose();
    }
}
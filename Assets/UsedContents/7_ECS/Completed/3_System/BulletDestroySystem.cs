using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

/// <summary>
/// ‘¬“x‚ªˆê’èˆÈ‰º‚É‚È‚Á‚½’e‚ğ”j‰ó‚·‚éSystem
/// ’e‚Ì¶¬‹y‚ÑˆÚ“®‚ÌSystem‚ğÀsŒãAÅŒã‚ÉÀs‚·‚é
/// </summary>
[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public partial struct BulletDestroySystem : ISystem
{
    EntityQuery _query;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        NativeArray<ComponentType> components = new NativeArray<ComponentType>(1, Allocator.Temp);
        components[0] = ComponentType.ReadOnly<BulletSpeedComponent>();

        _query = state.GetEntityQuery(components);
        components.Dispose();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
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
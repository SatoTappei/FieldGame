using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// MonoBehaviour���̓��͂��ꂽ�l�ɉ����Ēe�𔭎˂���System
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
            // �e����
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            new ShootTheBulletJob
            {
                _ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
                _pos = component._pos,
                _dir = component._dir,
            }.Run();

            // �e�����f�[�^�̃��Z�b�g
            component._active = false;
            component._pos = float3.zero;
            component._dir = float3.zero;
            SystemAPI.SetSingleton(component);
        }

        #region Job���g�pver
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

        //    // TODO:ShootTrigger���I�t�ɂ��鏈�����K�v
        //    //state.EntityManager.SetComponentEnabled<ShootTrigger>(holder, false);
        //}
        #endregion
    }
}

/// <summary>
/// �e�𐶐�����Job
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
        
        // SetComponent�Œǉ����悤�Ƃ���ƃG���[���o��
        _ecb.AddComponent(entity, new BulletSpeedComponent { _value = 5.0f });
        _ecb.AddComponent(entity, new BulletDirectionComponent { _value = _dir });
    }

    /// <summary>
    /// ���W�ɉ�������]�Ƒ傫�����f�t�H���g��Transform���擾����
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
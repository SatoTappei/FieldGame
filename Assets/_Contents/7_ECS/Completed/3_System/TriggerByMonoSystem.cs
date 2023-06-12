using Unity.Entities;

/// <summary>
/// GameObject���̒l���Q�Ƃ��Ēe�𔭎˂���ׂɎQ�Ƃ�����System
/// �l���Q�Ƃ���̂ōŏ��Ɏ��s����
/// </summary>
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
public partial class TriggerByMonoSystem : SystemBase
{
    protected override void OnCreate()
    {
        EntityManager.CreateSingleton<BulletSpawnComponent>();
    }

    protected override void OnDestroy()
    {
        Entity entity = SystemAPI.GetSingletonEntity<BulletSpawnComponent>();
        EntityManager.DestroyEntity(entity);
    }

    protected override void OnUpdate()
    {
        // �e�����f�[�^���擾�o�����炻�̃f�[�^��ݒ肷��
        if (TriggerByMonoBroker.Instance.TryGetShootData(out ShootData data))
        {
            BulletSpawnComponent component = SystemAPI.GetSingleton<BulletSpawnComponent>();
            component._pos = data.Pos;
            component._dir = data.Dir;
            component._type = data.Type;
            component._active = true;

            SystemAPI.SetSingleton(component);
        }
    }
}
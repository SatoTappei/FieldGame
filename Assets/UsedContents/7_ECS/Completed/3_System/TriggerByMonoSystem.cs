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
        EntityManager.CreateSingleton<PlayerBulletSpawnComponent>();
    }

    protected override void OnDestroy()
    {
        Entity entity = SystemAPI.GetSingletonEntity<PlayerBulletSpawnComponent>();
        EntityManager.DestroyEntity(entity);
    }

    protected override void OnUpdate()
    {
        // �e�����f�[�^���擾�o�����炻�̃f�[�^��ݒ肷��
        if (TriggerByMonoBroker.Instance.TryGetShootData(out ShootData data))
        {
            PlayerBulletSpawnComponent component = SystemAPI.GetSingleton<PlayerBulletSpawnComponent>();
            component._active = true;
            component._pos = data.Pos;
            component._dir = data.Dir;
            SystemAPI.SetSingleton(component);
        }
    }
}
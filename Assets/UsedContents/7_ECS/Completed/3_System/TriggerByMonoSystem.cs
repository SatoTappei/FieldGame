using Unity.Entities;

/// <summary>
/// GameObject側の値を参照して弾を発射する為に参照を持つSystem
/// 値を参照するので最初に実行する
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
        // 弾生成データを取得出来たらそのデータを設定する
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
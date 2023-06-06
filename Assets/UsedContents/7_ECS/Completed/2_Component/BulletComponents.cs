using Unity.Entities;
using Unity.Mathematics;

public struct BulletSpeedComponent : IComponentData
{
    public float _value;
}

public struct BulletDirectionComponent : IComponentData
{
    public float3 _value;
}

public struct BulletAccelerationComponent : IComponentData
{
    // 未使用
}

public struct BulletHolderComponent : IComponentData
{
    public Entity _prototype;
}

public struct RandomValueComponent : IComponentData
{
    public Random _value;
    public uint _seed;
}

/// <summary>
/// GameObject側からプレイヤーの弾を発射する際のデータを保持するComponent
/// Singletonで扱う
/// </summary>
public struct PlayerBulletSpawnComponent : IComponentData
{
    public float3 _pos;
    public float3 _dir;
    /// <summary>
    /// この値がtrueの場合は発射する
    /// </summary>
    public bool _active;
}
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

public struct BulletHolderComponent : IComponentData
{
    public Entity _playerPrototype;
    public Entity _enemyPrototype;
}


public struct RandomValueComponent : IComponentData
{
    public Random _value;
    public uint _seed;
}

/// <summary>
/// GameObject‘¤‚©‚ç’e‚ğ”­Ë‚·‚éÛ‚Ìƒf[ƒ^‚ğ•Û‚·‚éComponent
/// Singleton‚Åˆµ‚¤
/// </summary>
public struct BulletSpawnComponent : IComponentData
{
    public float3 _pos;
    public float3 _dir;
    public ShootData.BulletType _type;
    public bool _active;
}
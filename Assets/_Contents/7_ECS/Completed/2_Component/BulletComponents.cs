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
/// GameObject������e�𔭎˂���ۂ̃f�[�^��ێ�����Component
/// Singleton�ň���
/// </summary>
public struct BulletSpawnComponent : IComponentData
{
    public float3 _pos;
    public float3 _dir;
    public ShootData.BulletType _type;
    public bool _active;
}
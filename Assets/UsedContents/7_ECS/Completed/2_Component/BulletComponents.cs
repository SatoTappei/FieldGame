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
    public Entity _prototype;
}

/// <summary>
/// GameObject������v���C���[�̒e�𔭎˂���ۂ̃f�[�^��ێ�����Component
/// Singleton�ň���
/// </summary>
public struct PlayerBulletSpawnComponent : IComponentData
{
    public float3 _pos;
    public float3 _dir;
    /// <summary>
    /// ���̒l��true�̏ꍇ�͔��˂���
    /// </summary>
    public bool _active;
}
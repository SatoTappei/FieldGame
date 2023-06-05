using Unity.Entities;
using Unity.Transforms;

// 未使用

/// <summary>
/// 弾を移動させるJobで使用する際に使用するのに必要な値のAspect
/// </summary>
public readonly partial struct BulletMoveAspect : IAspect
{
    public readonly RefRO<BulletSpeedComponent> _speed;
    public readonly RefRO<BulletDirectionComponent> _dir;
    public readonly TransformAspect _transform;
}

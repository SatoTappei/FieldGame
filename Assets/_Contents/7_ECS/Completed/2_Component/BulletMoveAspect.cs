using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// 弾を移動させるJobで使用する際に使用するのに必要な値のAspect
/// </summary>
public readonly partial struct BulletMoveAspect : IAspect
{
    public readonly RefRW<BulletSpeedComponent> _speed;
    public readonly RefRO<BulletDirectionComponent> _dir;
    public readonly TransformAspect _transform;
}
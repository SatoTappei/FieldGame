using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// �e���ړ�������Job�Ŏg�p����ۂɎg�p����̂ɕK�v�Ȓl��Aspect
/// </summary>
public readonly partial struct BulletMoveAspect : IAspect
{
    public readonly RefRW<BulletSpeedComponent> _speed;
    public readonly RefRO<BulletDirectionComponent> _dir;
    public readonly TransformAspect _transform;
}
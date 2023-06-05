using Unity.Entities;
using Unity.Transforms;

// ���g�p

/// <summary>
/// �e���ړ�������Job�Ŏg�p����ۂɎg�p����̂ɕK�v�Ȓl��Aspect
/// </summary>
public readonly partial struct BulletMoveAspect : IAspect
{
    public readonly RefRO<BulletSpeedComponent> _speed;
    public readonly RefRO<BulletDirectionComponent> _dir;
    public readonly TransformAspect _transform;
}

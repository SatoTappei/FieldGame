using Unity.Entities;

// ���g�p

/// <summary>
/// �e�𔭎˂���Entity��Aspect
/// </summary>
public readonly partial struct BulletHolderAspect : IAspect
{
    public readonly RefRO<BulletHolderComponent> _holder;
}

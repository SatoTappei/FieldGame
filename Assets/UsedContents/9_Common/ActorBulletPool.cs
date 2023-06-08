using UniRx.Toolkit;
using UnityEngine;

/// <summary>
/// キャラクターが発射する弾のプールを実装するクラス
/// </summary>
public class ActorBulletPool : ObjectPool<ActorBullet>
{
    readonly ActorBullet _origin;
    readonly Transform _parent;

    public ActorBulletPool(ActorBullet origin, string poolName)
    {
        _origin = origin;
        _origin.gameObject.SetActive(false);

        _parent = new GameObject(poolName).transform;
    }

    protected override ActorBullet CreateInstance()
    {
        ActorBullet bullet = Object.Instantiate(_origin, _parent);
        bullet.InitOnCreate(this);
        return bullet;
    }

    protected override void OnBeforeRent(ActorBullet instance)
    {
        base.OnBeforeRent(instance);
    }

    protected override void OnBeforeReturn(ActorBullet instance)
    {
        base.OnBeforeReturn(instance);
    }
}

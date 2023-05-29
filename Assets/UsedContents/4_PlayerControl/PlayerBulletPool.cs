using UniRx.Toolkit;
using UnityEngine;

/// <summary>
/// プレイヤーが発射する弾のプールを実装するクラス
/// </summary>
public class PlayerBulletPool : ObjectPool<PlayerBullet>
{
    readonly PlayerBullet _origin;
    readonly Transform _parent;

    public PlayerBulletPool(PlayerBullet origin, Transform parent)
    {
        _origin = origin;
        _origin.gameObject.SetActive(false);

        _parent = parent;
    }

    protected override PlayerBullet CreateInstance()
    {
        PlayerBullet bullet = Object.Instantiate(_origin, _parent);
        bullet.InitOnCreate(this);
        return bullet;
    }

    protected override void OnBeforeRent(PlayerBullet instance)
    {
        base.OnBeforeRent(instance);
    }

    protected override void OnBeforeReturn(PlayerBullet instance)
    {
        base.OnBeforeReturn(instance);
    }
}

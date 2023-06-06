using UnityEngine;
using System;

/// <summary>
/// プレイヤーの攻撃に関する処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerFireBehavior : IInputActionRegistrable, IDisposable
{
    [Header("発射する弾用")]
    [SerializeField] Transform _model;
    [SerializeField] Transform _muzzle;
    [Header("攻撃のレート")]
    [SerializeField] float _attackRate = 0.33f;
    [Header("攻撃時に再生するParticle")]
    [SerializeField] ParticleSystem _fireParticle;
    [Header("プレイヤーの弾のプーリング")]
    [SerializeField] PlayerBullet _playerBullet;
    [SerializeField] Transform _pool;

    PlayerBulletPool _bulletPool;

    float _time;
    /// <summary>
    /// InputSystemに登録して入力のオンオフで攻撃中フラグを切り替える
    /// </summary>
    bool _isFiring;

    public void RegisterInputAction(PlayerInputRegister register)
    {
        register.OnFire += () => OpenFire();
        register.OnFireCanceled += () => _isFiring = false;
    }

    public void InitOnAwake()
    {
        _bulletPool = new(_playerBullet, _pool);
    }

    /// <summary>
    /// 最初の1発は射撃ボタンを押したタイミングで必ず発射される
    /// 射撃ボタンを連打することで攻撃レート以上の早さで発射可能
    /// </summary>
    void OpenFire()
    {
        _isFiring = true;
        _time = _attackRate;
    }

    public void Update()
    {
        _time += Time.deltaTime;

        if (_isFiring && _time > _attackRate)
        {
            _time = 0;
            GameManager.Instance.AudioModule.PlaySE(AudioType.SE_Fire);
            _fireParticle.Play();

            TriggerByMonoBroker.Instance.AddShootData(_muzzle.position, _model.forward);

            PlayerBullet bullet = _bulletPool.Rent();
            bullet.OnRent(_model, _muzzle.position);
        }
    }

    public void Dispose()
    {
        _bulletPool.Clear();
    }
}

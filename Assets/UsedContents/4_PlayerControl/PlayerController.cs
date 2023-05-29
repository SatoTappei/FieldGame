using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 各振る舞いのクラスを用いてプレイヤーを制御するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerFireBehavior _playerFireBehavior;
    [SerializeField] PlayerMoveBehavior _playerMoveBehavior;
    [SerializeField] CameraControlModule _cameraControlModule;
    [SerializeField] PlayerAnimModule _animModule;
    [SerializeField] PlayerLifePointModule _lifePointModule;
    PlayerInputRegister _inputActionRegister;
    Transform _transform;

    void Awake()
    {
        InitOnAwake();
    }

    void InitOnAwake()
    {
        _inputActionRegister = new PlayerInputRegister(gameObject);
        _playerMoveBehavior.RegisterInputAction(_inputActionRegister);
        _playerFireBehavior.RegisterInputAction(_inputActionRegister);
        _cameraControlModule.RegisterInputAction(_inputActionRegister);
        _animModule.RegisterInputAction(_inputActionRegister);

        _playerFireBehavior.InitOnAwake();
        _lifePointModule.InitOnAwake(transform);

        // ダメージを受けて体力が減少した際にダメージのアニメーションを再生する
        _lifePointModule.CurrentLifePoint.Skip(1)
            .Subscribe(_ => 
            {
                _animModule.PlayDamageAnim();
                GameManager.Instance.AudioModule.PlaySE(AudioType.SE_PlayerDamage);
            }).AddTo(gameObject);

        // ダメージを受けて体力が0になった際に死亡の演出を再生する
        _lifePointModule.CurrentLifePoint.Where(lifePoint => lifePoint == 0)
            .Subscribe(_ => Debug.Log("死亡")).AddTo(gameObject);

        this.UpdateAsObservable().Subscribe(_ => 
        {
            CameraMode mode = _cameraControlModule.CurrentCameraMode;
            _playerMoveBehavior.Update(_transform, mode);
            _playerFireBehavior.Update();
            _cameraControlModule.Update();
            _animModule.Update();
        });

        this.OnDestroyAsObservable().Subscribe(_ =>
        {
            _playerFireBehavior.Dispose();
        });

        _transform = transform;
    }
}

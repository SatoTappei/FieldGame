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
    [SerializeField] PlayerDefeatedBehavior _playerDefeatedBehavior;
    [SerializeField] CameraControlModule _cameraControlModule;
    [SerializeField] PlayerAnimModule _animModule;
    [SerializeField] PlayerLifePointModule _lifePointModule;
    [SerializeField] PlayerAimRaycastModule _aimRaycastModule;

    PlayerInputRegister _inputActionRegister;
    Transform _transform;

    void Awake()
    {
        InitOnAwake();
    }

    void InitOnAwake()
    {
        // InputSystemへの操作の登録
        _inputActionRegister = new PlayerInputRegister(gameObject);
        _playerMoveBehavior.RegisterInputAction(_inputActionRegister);
        _playerFireBehavior.RegisterInputAction(_inputActionRegister);
        _cameraControlModule.RegisterInputAction(_inputActionRegister);
        _animModule.RegisterInputAction(_inputActionRegister);
        
        // 各クラスの初期化
        _playerFireBehavior.InitOnAwake();
        _lifePointModule.InitOnAwake(transform);

        // ダメージを受けて体力が減少した際にダメージのアニメーションを再生する
        _lifePointModule.CurrentLifePoint.Skip(1).Subscribe(_ => 
        {
            _animModule.PlayDamageAnim();
            GameManager.Instance.AudioModule.PlaySE(AudioType.SE_PlayerDamage);
        }).AddTo(gameObject);

        // ダメージを受けて体力が0になった際に死亡の演出を再生する
        _lifePointModule.CurrentLifePoint.Where(lifePoint => lifePoint == 0)
        .Subscribe(_ => 
        {
            // 体力を全回復して位置を初期位置に戻す
            _playerDefeatedBehavior.Respawn(_transform);
            _lifePointModule.Reset();
        }).AddTo(gameObject);

        // 1フレーム毎の処理をしているのでオブジェクトを非表示にすれば正常に止まる
        this.UpdateAsObservable().Subscribe(_ => 
        {
            CameraMode mode = _cameraControlModule.CurrentCameraMode;
            _playerMoveBehavior.Update(_transform, mode);
            _playerFireBehavior.Update();
            _cameraControlModule.Update();
            _animModule.Update();
            _aimRaycastModule.Update(mode);
        });

        // ゲーム終了時にオブジェクトが破棄されるタイミングで後処理
        this.OnDestroyAsObservable().Subscribe(_ =>
        {
            _playerFireBehavior.Dispose();
        });

        _transform = transform;
    }

    void OnDrawGizmos()
    {
        _aimRaycastModule.Visualize();
    }
}

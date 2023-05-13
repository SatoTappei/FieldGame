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
    [SerializeField] PlayerFocusBehavior _playerFocusBehavior;
    InputActionRegister _inputActionRegister;
    Transform _transform;

    void Awake()
    {
        InitOnAwake();
    }

    void InitOnAwake()
    {
        _inputActionRegister = new InputActionRegister();
        _playerMoveBehavior.RegisterInputAction(_inputActionRegister);
        _playerFireBehavior.RegisterInputAction(_inputActionRegister);
        _playerFocusBehavior.RegisterInputAction(_inputActionRegister);

        this.UpdateAsObservable().Subscribe(_ => 
        {
            _playerMoveBehavior.Update(_transform);
            _playerFireBehavior.Update();
        });

        _transform = transform;
    }
}

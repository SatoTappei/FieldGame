using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �e�U�镑���̃N���X��p���ăv���C���[�𐧌䂷��N���X
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
        _inputActionRegister.OnMove += _playerMoveBehavior.UpdateVelocity;
        _inputActionRegister.OnMoveCanceled += _playerMoveBehavior.UpdateVelocity;
        _inputActionRegister.OnRun += () => _playerMoveBehavior.IsRunning = true;
        _inputActionRegister.OnRunCanceled += () => _playerMoveBehavior.IsRunning = false;
        _inputActionRegister.OnFire += () => _playerFireBehavior.IsFiring = true;
        _inputActionRegister.OnFireCanceled += () => _playerFireBehavior.IsFiring = false;
        _inputActionRegister.OnFocus += _playerFocusBehavior.FocusForward;

        this.UpdateAsObservable().Subscribe(_ => 
        {
            _playerMoveBehavior.Update(_transform);
            _playerFireBehavior.Update();
        });

        _transform = transform;
    }
}

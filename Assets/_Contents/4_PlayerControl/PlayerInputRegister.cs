using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// プレイヤーの操作を登録する用のクラス
/// </summary>
public class PlayerInputRegister
{
    PlayerInputActions _inputActions;

    public event UnityAction<InputAction.CallbackContext> OnMove;
    public event UnityAction<InputAction.CallbackContext> OnMoveCanceled;
    public event UnityAction<InputAction.CallbackContext> OnCameraMove;
    public event UnityAction<InputAction.CallbackContext> OnCameraMoveCanceled;
    public event UnityAction OnFire;
    public event UnityAction OnFireCanceled;
    public event UnityAction OnRun;
    public event UnityAction OnRunCanceled;
    public event UnityAction OnFocus;
    public event UnityAction OnFocusCanceled;

    public PlayerInputRegister(GameObject gameObject)
    {
        _inputActions = new();
        _inputActions.Enable();
        _inputActions.Player.Disable();
        _inputActions.Player.Move.performed += context => OnMove?.Invoke(context);
        _inputActions.Player.Move.canceled += context => OnMoveCanceled?.Invoke(context);
        _inputActions.Player.CameraMove.performed += context => OnCameraMove?.Invoke(context);
        _inputActions.Player.CameraMove.canceled += context => OnCameraMoveCanceled?.Invoke(context);
        _inputActions.Player.Fire.started += _ => OnFire?.Invoke();
        _inputActions.Player.Fire.canceled += _ => OnFireCanceled?.Invoke();
        _inputActions.Player.Run.started += _ => OnRun?.Invoke();
        _inputActions.Player.Run.canceled += _ => OnRunCanceled?.Invoke();
        _inputActions.Player.Focus.started += _ => OnFocus?.Invoke();
        _inputActions.Player.Focus.canceled += _ => OnFocusCanceled?.Invoke();

        MessageBroker.Default.Receive<InputTypeData>().Subscribe(data => 
        {
            if(data.Type == InputTypeData.InputType.Player)
            {
                _inputActions.Player.Enable();
                _inputActions.UI.Disable();
            }
            else if(data.Type == InputTypeData.InputType.UI)
            {
                _inputActions.UI.Enable();
                _inputActions.Player.Disable();
            }
            else
            {
                _inputActions.UI.Disable();
                _inputActions.Player.Disable();
            }
        }).AddTo(gameObject);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーのアニメーションを制御するクラス
/// </summary>
[System.Serializable]
public class PlayerAnimModule : IInputActionRegistrable
{
    /// <summary>
    /// このパラメータを操作することで、MoveとIdleのアニメーションが切り替わる
    /// 走る場合はこのパラメータに倍率をかけることでアニメーションを速くする
    /// </summary>
    static readonly string MoveAnimSpeedParamName = "Speed";

    static readonly string DamageAnimName = "Damage";

    [SerializeField] Animator _animator;
    [Header("走る際のアニメーションの速度の倍率")]
    [SerializeField] float _runSpeedParamMag = 2;

    float _moveAnimSpeedParam;
    bool _isRun;

    public void RegisterInputAction(PlayerInputRegister register)
    {
        register.OnMove += SetAnimParam;
        register.OnMoveCanceled += SetAnimParam;
        register.OnRun += () => _isRun = true;
        register.OnRunCanceled += () => _isRun = false;
    }

    void SetAnimParam(InputAction.CallbackContext context)
    {
        _moveAnimSpeedParam = context.ReadValue<Vector2>().sqrMagnitude;
    }

    /// <summary>
    /// 体力のReactivePropertyにSubscribeして呼び出す
    /// </summary>
    public void PlayDamageAnim() => _animator.Play(DamageAnimName);

    public void Update()
    {
        float param = _moveAnimSpeedParam * (_isRun ? _runSpeedParamMag : 1);
        _animator.SetFloat(MoveAnimSpeedParamName, param);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̃A�j���[�V�����𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class PlayerAnimModule : IInputActionRegistrable
{
    /// <summary>
    /// ���̃p�����[�^�𑀍삷�邱�ƂŁAMove��Idle�̃A�j���[�V�������؂�ւ��
    /// ����ꍇ�͂��̃p�����[�^�ɔ{���������邱�ƂŃA�j���[�V�����𑬂�����
    /// </summary>
    static readonly string MoveAnimSpeedParamName = "Speed";

    static readonly string DamageAnimName = "Damage";

    [SerializeField] Animator _animator;
    [Header("����ۂ̃A�j���[�V�����̑��x�̔{��")]
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
    /// �̗͂�ReactiveProperty��Subscribe���ČĂяo��
    /// </summary>
    public void PlayDamageAnim() => _animator.Play(DamageAnimName);

    public void Update()
    {
        float param = _moveAnimSpeedParam * (_isRun ? _runSpeedParamMag : 1);
        _animator.SetFloat(MoveAnimSpeedParamName, param);
    }
}

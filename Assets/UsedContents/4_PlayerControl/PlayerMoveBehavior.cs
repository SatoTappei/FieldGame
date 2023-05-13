using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̈ړ��Ɋւ��鏈�����s���N���X
/// </summary>
[System.Serializable]
public class PlayerMoveBehavior : IInputActionRegistrable
{
    [Header("�ړ����x")]
    [SerializeField] float _speed = 5.0f;
    [Header("����ۂ̈ړ����x�̔{��")]
    [SerializeField] float _runMag = 1.5f;

    /// <summary>
    /// ���͂Ɉړ����x����Z���Đ��K�������l
    /// </summary>
    Vector3 _velo;
    /// <summary>
    /// InputSystem�ɓo�^���ē��͂̃I���I�t�ňړ����t���O��؂�ւ���
    /// </summary>
    bool _isRunning;

    public void RegisterInputAction(InputActionRegister register)
    {
        register.OnMove += UpdateVelocity;
        register.OnMoveCanceled += UpdateVelocity;
        register.OnRun += () => _isRunning = true;
        register.OnRunCanceled += () => _isRunning = false;
    }

    public void Update(Transform transform)
    {
        float mag = _isRunning ? _runMag : 1;
        transform.Translate(_velo * Time.deltaTime * mag);
    }

    /// <summary>
    /// �ړ����x�̍X�V
    /// ���Ԋu�œ��͂��󂯎��悤InputSystem�ɓo�^�����
    /// </summary>
    void UpdateVelocity(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _velo = new Vector3(value.x, 0, value.y).normalized * _speed;
    }
}

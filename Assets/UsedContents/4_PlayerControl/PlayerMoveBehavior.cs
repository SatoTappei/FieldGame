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
    [Header("�ړ��ɉ����������Ɍ�������Transform")]
    [SerializeField] Transform _model;
    [Header("�������x")]
    [SerializeField] float _rotSpeed = 20;

    /// <summary>
    /// ���͂�i�ޕ���(���K���ς�)�ɕϊ������l
    /// </summary>
    Vector3 _dir;
    /// <summary>
    /// InputSystem�ɓo�^���ē��͂̃I���I�t�ő��蒆�t���O��؂�ւ���
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
        // �ړ�
        Vector3 movement = _dir * Time.deltaTime * _speed * (_isRunning ? _runMag : 1);
        transform.Translate(movement);

        // �ړ������Ɍ�������
        if (movement != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(_dir, Vector3.up);
            _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
        }
    }

    /// <summary>
    /// �ړ����x�̍X�V
    /// ���Ԋu�œ��͂��󂯎��悤InputSystem�ɓo�^�����
    /// </summary>
    void UpdateVelocity(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.x, 0, value.y).normalized;
    }
}

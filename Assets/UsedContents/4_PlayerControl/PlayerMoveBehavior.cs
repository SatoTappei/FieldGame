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
    [Header("�ړ��ɉ����������ɐU���������I�u�W�F�N�g")]
    [SerializeField] Transform _model;
    [Header("�U��������x")]
    [SerializeField] float _rotSpeed = 20.0f;

    /// <summary>
    /// ���͂��ړ�����(���K���ς�)�ɕϊ������l
    /// </summary>
    Vector3 _dir;
    /// <summary>
    /// InputSystem�ɓo�^���ē��͂̃I���I�t�ő��蒆�t���O��؂�ւ���
    /// </summary>
    bool _isRunning;

    public void RegisterInputAction(PlayerInputRegister register)
    {
        register.OnMove += UpdateNormalizedDirection;
        register.OnMoveCanceled += UpdateNormalizedDirection;
        register.OnRun += () => _isRunning = true;
        register.OnRunCanceled += () => _isRunning = false;
    }

    public void Update(Transform transform, CameraMode mode)
    {
        // Y������]���Ƃ�����]
        Quaternion cameraRot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // �ړ�
        Vector3 movement = cameraRot * _dir * Time.deltaTime * _speed * (_isRunning ? _runMag : 1);
        transform.Translate(movement);

        // Freelook���ɂ̂݃v���C���[�̉�]���s��
        if (mode == CameraMode.Freelook)
        {
            if (movement != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(cameraRot * _dir, Vector3.up);
                _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
            }
        }
    }

    /// <summary>
    /// �ړ������̍X�V
    /// ���Ԋu�œ��͂��󂯎��悤InputSystem�ɓo�^�����
    /// </summary>
    void UpdateNormalizedDirection(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.x, 0, value.y).normalized;
    }
}

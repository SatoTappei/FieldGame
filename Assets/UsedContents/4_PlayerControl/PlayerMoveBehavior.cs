using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̈ړ��Ɋւ��鏈�����s���N���X
/// </summary>
[System.Serializable]
public class PlayerMoveBehavior : IInputActionRegistrable
{
    [SerializeField] Rigidbody _rigidbody;
    [Header("�ړ����x")]
    [SerializeField] float _speed = 5.0f;
    [Header("����ۂ̈ړ����x�̔{��")]
    [SerializeField] float _runMag = 1.5f;
    [Header("�ړ��ɉ����������ɐU���������I�u�W�F�N�g")]
    [SerializeField] Transform _model;
    [Header("�U��������x")]
    [SerializeField] float _rotSpeed = 20.0f;

    Vector3 _velo;
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

    public void Update(CameraMode mode)
    {
        // Y������]���Ƃ�����]
        Quaternion cameraRot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // ���x�̍X�V
        Vector3 velo = cameraRot * _dir * _speed * (_isRunning ? _runMag : 1);
        velo.y = _rigidbody.velocity.y;
        _velo = velo;

        // Freelook���ɂ͈ړ������Ɍ���
        if (mode == CameraMode.Freelook)
        {
            if (velo.x == 0 && velo.z == 0) return;

            Quaternion rot = Quaternion.LookRotation(cameraRot * _dir, Vector3.up);
            _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
        }
        // ADS���ɂ̓J�����̒���������
        else if(mode == CameraMode.ADS)
        {
            if (Mathf.Approximately(_model.rotation.y, Camera.main.transform.rotation.y)) return;

            Quaternion rot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            rot.x = 0;
            rot.z = 0;
            _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
        }
    }

    public void FixedUpdate()
    {
        _rigidbody.velocity = _velo;
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

    /// <summary>
    /// �ړ����x�������I��0�ɂ��鎖�ŁA�v���C���[�����X�|�[�������ۂɊ����Ă����̂�h��
    /// </summary>
    public void ResetIdleVelocity()
    {
        _isRunning = false;
        _dir = Vector3.zero;
    }
}

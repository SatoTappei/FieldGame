using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

/// <summary>
/// ���݂̃J�������[�h�̗񋓌^
/// CameraControlModule�ŕύX���s��
/// </summary>
public enum CameraMode
{
    Freelook,
    ADS,
}

/// <summary>
/// �v���C���[�̃J�����̑�����s���N���X
/// </summary>
[System.Serializable]
public class CameraControlModule : IInputActionRegistrable
{
    [Header("0:Freelook 1:ADS")]
    [SerializeField] CinemachineVirtualCamera[] _cameras;
    [Header("VCam��Follow���Ă���J��������œ������I�u�W�F�N�g")]
    [SerializeField] Transform _horiFollowTarget;
    [SerializeField] Transform _vertFollowTarget;
    [Header("���x")]
    [SerializeField] int _sensitivity = 160;
    [Header("X�������̊p�x�͈̔�")]
    [SerializeField] float _angleMax = 45.0f;
    [SerializeField] float _angleMin = -45.0f;
    [Header("Freelook�ɂ����ۂɉ�]��ǂݎ��I�u�W�F�N�g")]
    [SerializeField] Transform _model;

    /// <summary>
    /// ���͂��J�����̈ړ�����(���K���ς�)�ɕϊ������l
    /// </summary>
    Vector3 _dir;
    /// <summary>
    /// ���݂̃J�������[�h
    /// ���̃t���[���ňړ����X�V����ۂɎQ�Ƃ����
    /// </summary>
    public CameraMode CurrentCameraMode { get; private set; }

    public void RegisterInputAction(PlayerInputRegister register)
    {
        register.OnCameraMove += UpdateNormalizedDirection;
        register.OnCameraMoveCanceled += UpdateNormalizedDirection;
        register.OnFocus += SwitchCamera;
        register.OnFocusCanceled += SwitchCamera;
    }

    public void Update()
    {
        // ADS����FreeLook�J�����̑�������Ȃ�
        if (CurrentCameraMode == CameraMode.ADS) return;

        // �J��������(����)
        Vector3 cameraMovement = _dir * Time.deltaTime * _sensitivity;
        _horiFollowTarget.Rotate(new Vector3(0, cameraMovement.y, 0));

        // �J��������(����)
        float rotX = _vertFollowTarget.localEulerAngles.x;
        rotX -= cameraMovement.x;
        if (rotX > 180) rotX -= 360;
        rotX = Mathf.Clamp(rotX, _angleMin, _angleMax);
        _vertFollowTarget.localEulerAngles = new Vector3(rotX, 0, 0);
    }

    void SwitchCamera()
    {
        // �g�p����J�����̕ύX(�J�������[�h�̐؂�ւ�)
        int index = (int)CurrentCameraMode;
        _cameras[index].Priority = 10;
        index = 1 - index;
        _cameras[index].Priority = 11;
        CurrentCameraMode = (CameraMode)index;

        // ADS�ɐ؂�ւ�����ۂɏc�����̉�]��0�ɖ߂�
        if (CurrentCameraMode == CameraMode.ADS)
        {
            _vertFollowTarget.localEulerAngles = Vector3.zero;
        }
    }

    /// <summary>
    /// �J�����̕����̍X�V
    /// ���Ԋu�œ��͂��󂯎��悤InputSystem�ɓo�^�����
    /// </summary>
    void UpdateNormalizedDirection(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.y, value.x, 0).normalized;
    }
}

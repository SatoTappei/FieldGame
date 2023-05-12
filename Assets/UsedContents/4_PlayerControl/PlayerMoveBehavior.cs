using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �v���C���[�̈ړ��Ɋւ��鏈�����s���N���X
/// </summary>
[System.Serializable]
public class PlayerMoveBehavior
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
    /// InputSystem�ɓo�^���đ���t���O��؂�ւ��邽�߂�SetGet�ǂ�������J����K�v������
    /// </summary>
    public bool IsRunning { get; set; }

    public void Update(Transform transform)
    {
        float mag = IsRunning ? _runMag : 1;
        transform.Translate(_velo * Time.deltaTime * mag);
    }

    /// <summary>
    /// �ړ����x�̍X�V
    /// ���Ԋu�œ��͂��󂯎��悤InputSystem�ɓo�^�����
    /// </summary>
    public void UpdateVelocity(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _velo = new Vector3(value.x, 0, value.y).normalized * _speed;
    }
}

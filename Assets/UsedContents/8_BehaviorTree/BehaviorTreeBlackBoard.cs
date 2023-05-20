using UnityEngine;

/// <summary>
/// BehaviorTree�Ŏg�p����e��f�[�^��ǂݏ�������N���X
/// </summary>
[System.Serializable]
public class BehaviorTreeBlackBoard
{
    /// <summary>
    /// �v���C���[�����m����Ԋu
    /// </summary>
    public static float DetectInterval = 0.1f;

    [SerializeField] Transform _transform;
    [SerializeField] Rigidbody _rigidbody;
    [Header("�U�����ɍĐ�����Particle")]
    [SerializeField] ParticleSystem _fireParticle;
    [Header("�v���C���[�������郌�C���[")]
    [SerializeField] LayerMask _playerLayer;
    [Header("�v���C���[�����m����͈�")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("���ˑ��x")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("�ړ����x")]
    [Range(0.01f, 9.0f)]
    [SerializeField] float _moveSpeed = 5.0f;

    public Transform Transform => _transform;
    public Rigidbody Rigidbody => _rigidbody;
    public ParticleSystem FireParticle => _fireParticle;
    public LayerMask PlayerLayer => _playerLayer;
    public float DetectRadius => _detectRadius;
    public float FireRate => _fireRate;
    public float MoveSpeed => _moveSpeed;
}

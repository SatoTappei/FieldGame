using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviorTree�Ŏg�p����e��f�[�^��ǂݏ������鍕�N���X
/// </summary>
[System.Serializable]
public class BehaviorTreeBlackBoard
{
    /// <summary>
    /// RayCast�n��p���ăv���C���[�����m����Ԋu
    /// �^�C�}�[�f�R���[�^�Ɗe��m�[�h�́A���Ԋu��RayCast���鏈���Ɏg�p�����
    /// </summary>
    public static readonly float DetectInterval = 0.1f;

    [SerializeField] Transform _player;
    [SerializeField] Transform _transform;
    [SerializeField] Transform _model;
    [SerializeField] Rigidbody _rigidbody;
    [Header("�U�����ɍĐ�����Particle")]
    [SerializeField] ParticleSystem _fireParticle;
    [Header("�v���C���[�������郌�C���[")]
    [SerializeField] LayerMask _playerLayer;
    [Header("�v���C���[�����m����͈�")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("�U���J�n�͈�")]
    [SerializeField] float _fireRadius = 3.0f;
    [Header("�U�����x")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("�ړ����x")]
    [Range(0.01f, 9.0f)]
    [SerializeField] float _moveSpeed = 5.0f;
    [Header("��]���x")]
    [SerializeField] float _rotSpeed = 10.0f;
    [Header("�̗�(�_���[�W1�̒e�ɑ΂���)")]
    [SerializeField] int _lifePoint = 3;

    public Transform Player => _player;
    public Transform Transform => _transform;
    public Transform Model => _model;
    public Rigidbody Rigidbody => _rigidbody;
    public ParticleSystem FireParticle => _fireParticle;
    public LayerMask PlayerLayer => _playerLayer;
    public float DetectRadius => _detectRadius;
    public float FireRadius => _fireRadius;
    public float FireRate => _fireRate;
    public float MoveSpeed => _moveSpeed;
    public float RotSpeed => _rotSpeed;

    Queue<Vector3> _path;

    /// <summary>
    /// �o�H�T���������ʂ�Path���m�[�h����ǂݏ�������
    /// Vector3�ŕێ����Ă���̂�A*�ȊO�ɂ��Ή����Ă���
    /// </summary>
    public Queue<Vector3> Path
    {
        get
        {
            if (_path == null)
            {
                Debug.LogWarning("Path��null�Ȃ̂Ō��ݒn�̍��W�Ɉړ�����Path���쐬���ĕԂ���");
                Queue<Vector3> path = new();
                path.Enqueue(Transform.position);
                return path;
            }
            else
            {
                return _path;
            }
        }
        set
        {
            _path = value;
        }
    }

    /// <summary>
    /// EnemyHealthModule���ǂݎ���Čv�Z���ď�������
    /// �e�m�[�h�ł͓ǂݎ�肵�����Ȃ�
    /// </summary>
    public int LifePoint { get => _lifePoint; set => _lifePoint = value; }
}

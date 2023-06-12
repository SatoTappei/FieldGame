using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonkeyModelBlackBoard : MonoBehaviour
{
    static readonly float VisibleRayOffset = 0.5f;
    static readonly float DetectInterval = 0.1f;

    [Header("�L�����N�^�[�̃��f��(Renderer)")]
    [SerializeField] Transform _model;
    [Header("�v���C���[�̃^�O")]
    [SerializeField] string _playerTag = "Player";
    [Header("�U�����x")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("�U���J�n�͈�")]
    [SerializeField] float _fireRadius = 3.0f;
    [Header("�ړ����x")]
    [Range(0.01f, 9.0f)]
    [SerializeField] float _moveSpeed = 5.0f;
    [Header("��]���x")]
    [SerializeField] float _rotSpeed = 10.0f;
    [Header("�v���C���[���ǂȂǂ̏�Q���̃��C���[")]
    [SerializeField] LayerMask _playerDetectLayer;
    [Header("�v���C���[�����m����͈�")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("�f�o�b�O�p�̃��O��\������")]
    [SerializeField] bool _debugLog = true;

    Transform _transform;
    Transform _player;
    Rigidbody _rigidbody;

    public Transform Transform => _transform;
    public Transform Player => _player;
    public Transform Model => _model;
    public Rigidbody Rigidbody => _rigidbody;
    public LayerMask PlayerDetectLayer => _playerDetectLayer;
    public string PlayerTag => _playerTag;
    public float PlayerVisibleRayOffset => VisibleRayOffset;
    public float PlayerDetectInterval => DetectInterval;
    public float FireRate => _fireRate;
    public float FireRadius => _fireRadius;
    public float MoveSpeed => _moveSpeed;
    public float RotSpeed => _rotSpeed;
    public float DetectRadius  => _detectRadius;
    public bool DebugLog => _debugLog;

    void Start()
    {
        _transform = transform;
        _player = GameObject.FindGameObjectWithTag(_playerTag).transform;
        _rigidbody = GetComponent<Rigidbody>();
    }
}

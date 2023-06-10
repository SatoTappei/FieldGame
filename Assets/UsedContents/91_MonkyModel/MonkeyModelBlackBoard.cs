using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MonkeyModelBlackBoard : MonoBehaviour
{
    static readonly float VisibleRayOffset = 0.5f;
    static readonly float DetectInterval = 0.1f;

    [Header("キャラクターのモデル(Renderer)")]
    [SerializeField] Transform _model;
    [Header("プレイヤーのタグ")]
    [SerializeField] string _playerTag = "Player";
    [Header("攻撃速度")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("攻撃開始範囲")]
    [SerializeField] float _fireRadius = 3.0f;
    [Header("移動速度")]
    [Range(0.01f, 9.0f)]
    [SerializeField] float _moveSpeed = 5.0f;
    [Header("回転速度")]
    [SerializeField] float _rotSpeed = 10.0f;
    [Header("プレイヤー＆壁などの障害物のレイヤー")]
    [SerializeField] LayerMask _playerDetectLayer;
    [Header("プレイヤーを検知する範囲")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("デバッグ用のログを表示する")]
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

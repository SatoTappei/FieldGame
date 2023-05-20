using UnityEngine;

/// <summary>
/// BehaviorTreeで使用する各種データを読み書きするクラス
/// </summary>
[System.Serializable]
public class BehaviorTreeBlackBoard
{
    /// <summary>
    /// プレイヤーを検知する間隔
    /// </summary>
    public static float DetectInterval = 0.1f;

    [SerializeField] Transform _transform;
    [SerializeField] Rigidbody _rigidbody;
    [Header("攻撃時に再生するParticle")]
    [SerializeField] ParticleSystem _fireParticle;
    [Header("プレイヤーが属するレイヤー")]
    [SerializeField] LayerMask _playerLayer;
    [Header("プレイヤーを検知する範囲")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("発射速度")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("移動速度")]
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

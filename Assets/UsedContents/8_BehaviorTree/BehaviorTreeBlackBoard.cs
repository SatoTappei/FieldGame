using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviorTreeで使用する各種データを読み書きする黒板クラス
/// </summary>
[System.Serializable]
public class BehaviorTreeBlackBoard
{
    /// <summary>
    /// RayCast系を用いてプレイヤーを検知する間隔
    /// タイマーデコレータと各種ノードの、一定間隔でRayCastする処理に使用される
    /// </summary>
    public static readonly float DetectInterval = 0.1f;

    [SerializeField] Transform _player;
    [SerializeField] Transform _transform;
    [SerializeField] Transform _model;
    [SerializeField] Rigidbody _rigidbody;
    [Header("攻撃時に再生するParticle")]
    [SerializeField] ParticleSystem _fireParticle;
    [Header("プレイヤーが属するレイヤー")]
    [SerializeField] LayerMask _playerLayer;
    [Header("プレイヤーを検知する範囲")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("攻撃開始範囲")]
    [SerializeField] float _fireRadius = 3.0f;
    [Header("攻撃速度")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("移動速度")]
    [Range(0.01f, 9.0f)]
    [SerializeField] float _moveSpeed = 5.0f;

    public Transform Player => _player;
    public Transform Transform => _transform;
    public Rigidbody Rigidbody => _rigidbody;
    public ParticleSystem FireParticle => _fireParticle;
    public LayerMask PlayerLayer => _playerLayer;
    public float DetectRadius => _detectRadius;
    public float FireRadius => _fireRadius;
    public float FireRate => _fireRate;
    public float MoveSpeed => _moveSpeed;

    Queue<Vector3> _path;

    /// <summary>
    /// 経路探索した結果のPathをノードから読み書きする
    /// Vector3で保持しているのでA*以外にも対応している
    /// </summary>
    public Queue<Vector3> Path
    {
        get
        {
            if (_path == null)
            {
                Debug.LogWarning("Pathがnullなので現在地の座標に移動するPathを作成して返した");
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
}

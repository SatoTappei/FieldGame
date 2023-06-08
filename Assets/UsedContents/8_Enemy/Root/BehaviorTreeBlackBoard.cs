using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviorTreeで使用する各種データを読み書きする黒板クラス
/// </summary>
[System.Serializable]
public class BehaviorTreeBlackBoard
{
    /// <summary>
    /// 移動中にプレイヤーを視認するためのRayを撃つ高さ
    /// Modelの高さを1として値を設定する
    /// </summary>
    static readonly float VisibleRayOffset = 0.5f;

    /// <summary>
    /// RayCast系を用いてプレイヤーを検知する間隔
    /// タイマーデコレータと各種ノードの、一定間隔でRayCastする処理に使用される
    /// </summary>
    static readonly float DetectInterval = 0.1f;

    [SerializeField] Transform _player;
    [SerializeField] Transform _transform;
    [SerializeField] Transform _model;
    [SerializeField] Rigidbody _rigidbody;
    [Header("攻撃時に再生するParticle")]
    [SerializeField] ParticleSystem _fireParticle;
    [Header("攻撃用の弾(コライダーのみ)")]
    [SerializeField] ActorBullet _bullet;
    [Header("プレイヤーと障害物のレイヤーを指定する")]
    [SerializeField] LayerMask _playerDetectLayer;
    [Header("プレイヤーを検知する範囲")]
    [SerializeField] float _detectRadius = 5.0f;
    [Header("攻撃開始範囲")]
    [SerializeField] float _fireRadius = 3.0f;
    [Header("攻撃速度")]
    [SerializeField] float _fireRate = 1.0f;
    [Header("移動速度")]
    [Range(0.01f, 9.0f)]
    [SerializeField] float _moveSpeed = 5.0f;
    [Header("回転速度")]
    [SerializeField] float _rotSpeed = 10.0f;
    [Header("体力(ダメージ1の弾に対して)")]
    [SerializeField] int _lifePoint = 3;

    public Transform Player => _player;
    public Transform Transform => _transform;
    public Transform Model => _model;
    public Rigidbody Rigidbody => _rigidbody;
    public ParticleSystem FireParticle => _fireParticle;
    public ActorBullet Bullet => _bullet;
    public LayerMask PlayerDetectLayer => _playerDetectLayer;
    public float DetectRadius => _detectRadius;
    public float FireRadius => _fireRadius;
    public float FireRate => _fireRate;
    public float MoveSpeed => _moveSpeed;
    public float RotSpeed => _rotSpeed;
    public float PlayerVisibleRayOffset => VisibleRayOffset;
    public float PlayerDetectInterval => DetectInterval;

    Stack<Vector3> _path;

    /// <summary>
    /// 経路探索した結果のPathをノードから読み書きする
    /// Vector3で保持しているのでA*以外にも対応している
    /// </summary>
    public Stack<Vector3> Path
    {
        get
        {
            if (_path == null)
            {
                Debug.LogWarning("Pathがnullなので現在地の座標に移動するPathを作成して返した");
                Stack<Vector3> path = new();
                path.Push(Transform.position);
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
    /// EnemyLifePointModuleが読み取って計算して書き込む
    /// 各ノードでは読み取りしかしない
    /// </summary>
    public int LifePoint { get => _lifePoint; set => _lifePoint = value; }
}

using UnityEngine;
using System.Buffers;

/// <summary>
/// 経路探索の結果を用いて移動するアクションノード
/// </summary>
public class MoveByPathfindingAction : BehaviorTreeNode
{
    /// <summary>
    /// 移動中にプレイヤーを視認するためのRayを撃つ高さ
    /// Modelの高さを1として値を設定する
    /// </summary>
    public static readonly float PlayerVisibleRayOffset = 0.5f;
    /// <summary>
    /// 目的地に到着したとみなす距離
    /// この値は移動速度を速くした場合、調整しないといけない
    /// </summary>
    static readonly float Approximately = 0.3f;
    /// <summary>
    /// 何らかの理由で移動が出来なくなった場合に強制的に移動をキャンセルするための時間制限
    /// 強制的に成功を返す
    /// </summary>
    static readonly float TimeLimit = 12.0f;

    Vector3 _targetPos;
    float _playerDetectTimer;
    float _timeUpTimer;

    public MoveByPathfindingAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
        _targetPos = BlackBoard.Path.Dequeue();
        _playerDetectTimer = 0;
        _timeUpTimer = 0;
    }

    protected override void OnExit()
    {
        // 移動が完了もしくはキャンセルされた場合は黒板の経路を消す
        BlackBoard.Path = null;
    }

    protected override State OnStay()
    {
        // 一定間隔で攻撃開始範囲にプレイヤーがいるか検知
        _playerDetectTimer += Time.deltaTime;
        if (_playerDetectTimer > BehaviorTreeBlackBoard.DetectInterval)
        {
            _playerDetectTimer = 0;
            if (IsPlayerWithInFireRadius()) return State.Success;
        }
        // 時間制限で強制的に移動をキャンセル
        _timeUpTimer += Time.deltaTime;
        if (_timeUpTimer > TimeLimit)
        {
            Debug.LogWarning("時間切れで移動を強制的にキャンセル: " + BlackBoard.Transform.name);
            return State.Success;
        }

        // 移動
        if ((_targetPos - BlackBoard.Transform.position).sqrMagnitude <= Approximately)
        {
            // 黒板が保持している経路から次の地点を取得出来なかった場合は移動完了なので成功を返す
            if (!BlackBoard.Path.TryDequeue(out _targetPos)) return State.Success;
        }
        else
        {
            MoveToTarget();
        }

        return State.Running;
    }

    /// <summary>
    /// 攻撃開始範囲と同じ長さかつ、障害物を無視してプレイヤーにしかヒットしないRayを撃つ
    /// このRayがヒットした場合はプレイヤーを視認している
    /// </summary>
    bool IsPlayerWithInFireRadius()
    {
        RaycastHit[] hits = ArrayPool<RaycastHit>.Shared.Rent(1);
        Vector3 rayOrigin = BlackBoard.Transform.position;
        rayOrigin.y += PlayerVisibleRayOffset;
        Vector3 rayDir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;
        rayDir.y = 0;
        int count = Physics.RaycastNonAlloc(rayOrigin, rayDir, hits, BlackBoard.FireRadius, BlackBoard.PlayerLayer);
        ArrayPool<RaycastHit>.Shared.Return(hits, true);

        return count > 0;
    }

    void MoveToTarget()
    {
        Vector3 velo = (_targetPos - BlackBoard.Transform.position).normalized * BlackBoard.MoveSpeed;
        velo.y = BlackBoard.Rigidbody.velocity.y;
        BlackBoard.Rigidbody.velocity = velo;
    }
}

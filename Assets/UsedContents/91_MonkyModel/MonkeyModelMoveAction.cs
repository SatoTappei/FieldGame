using UnityEngine;

public class MonkeyModelMoveAction : MonkeyModelTreeNode
{
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

    float _playerDetectTimer;
    float _timeUpTimer;

    public MonkeyModelMoveAction(string nodeName, MonkeyModelBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
        _playerDetectTimer = 0;
        _timeUpTimer = 0;
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // 一定間隔で攻撃開始範囲にプレイヤーがいるか検知
        _playerDetectTimer += Time.deltaTime;
        if (_playerDetectTimer > BlackBoard.PlayerDetectInterval)
        {
            _playerDetectTimer = 0;
            if (IsPlayerWithInFireRadius()) return State.Success;
        }
        // 時間制限で強制的に移動をキャンセル
        _timeUpTimer += Time.deltaTime;
        if (_timeUpTimer > TimeLimit)
        {
            if (BlackBoard.DebugLog)
            {
                Debug.LogWarning("時間切れで移動を強制的にキャンセル: " + BlackBoard.Transform.name);
            }

            return State.Success;
        }

        // 移動
        if (IsArrivalTargetPos())
        {
            return State.Success;
        }
        else
        {
            MoveAndRotate();
        }

        return State.Running;
    }

    /// <summary>
    /// 攻撃開始範囲と同じ長さかつ、障害物とプレイヤーにしかヒットしないRayを飛ばす
    /// このRayがヒットした場合はプレイヤーを視認している
    /// </summary>
    bool IsPlayerWithInFireRadius()
    {
        Vector3 rayOrigin = BlackBoard.Transform.position;
        rayOrigin.y += BlackBoard.PlayerVisibleRayOffset;
        Vector3 rayDir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;
        rayDir.y = 0;
        bool rayHit = Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit,
            BlackBoard.FireRadius, BlackBoard.PlayerDetectLayer);

        if (rayHit)
        {
            return hit.collider.CompareTag(BlackBoard.PlayerTag);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// グリッドがプレイヤーの高さを基準に敷かれるので、Y座標を0にして比較することで
    /// 高低差がある場合でも距離の比較が出来る
    /// </summary>
    bool IsArrivalTargetPos()
    {
        Vector3 tp = new Vector3(BlackBoard.Player.position.x, 0, BlackBoard.Player.position.z);
        Vector3 btp = new Vector3(BlackBoard.Transform.position.x, 0, BlackBoard.Transform.position.z);
        return (tp - btp).sqrMagnitude <= Approximately;
    }

    void MoveAndRotate()
    {
        Vector3 dir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;

        // 移動
        Vector3 velo = dir * BlackBoard.MoveSpeed;
        velo.y = BlackBoard.Rigidbody.velocity.y;
        BlackBoard.Rigidbody.velocity = velo;

        // 回転
        Quaternion playerRot = Quaternion.AngleAxis(BlackBoard.Player.position.y, Vector3.up);
        Quaternion rot = Quaternion.LookRotation(playerRot * dir, Vector3.up);
        rot.x = 0;
        rot.z = 0;
        BlackBoard.Model.rotation = Quaternion.Lerp(BlackBoard.Model.rotation, rot,
            Time.deltaTime * BlackBoard.RotSpeed);
    }
}

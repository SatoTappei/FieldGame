using System.Buffers;
using UnityEngine;

/// <summary>
/// プレイヤーを検知するアクションノード
/// </summary>
public class DetectPlayerAction : BehaviorTreeNode
{
    public DetectPlayerAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // 長さ1以上の配列を借りてきてプレイヤーを検知する
        Collider[] hits = ArrayPool<Collider>.Shared.Rent(1);
        int count = Physics.OverlapSphereNonAlloc(BlackBoard.Transform.position,
            BlackBoard.DetectRadius, hits, BlackBoard.PlayerLayer);
        ArrayPool<Collider>.Shared.Return(hits, true);

        return count > 0 ? State.Success : State.Failure;
    }
}

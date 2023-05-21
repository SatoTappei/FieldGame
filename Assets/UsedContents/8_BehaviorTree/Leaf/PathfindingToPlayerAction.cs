using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーへの経路を探索するアクションノード
/// このノードで探索したパスを用いて移動を行う
/// </summary>
public class PathfindingToPlayerAction : BehaviorTreeNode
{
    public PathfindingToPlayerAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // 経路探索のクラスで求めた経路を黒板に書き込む
        Queue<Vector3> path = TempPathfindingSystem.Instance.GetPath(BlackBoard.Transform.position);
        BlackBoard.Path = path;

        return State.Success;
    }
}

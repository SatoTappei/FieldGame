using UnityEngine;

/// <summary>
/// プレイヤーへの経路を探索するアクションノード
/// このノードで探索したパスを用いて移動を行う
/// </summary>
public class PathfindingToPlayerAction : BehaviorTreeNode
{
    public PathfindingToPlayerAction(string nodeName) : base(nodeName) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // TODO:求められなかったら失敗を返すようにする
        Debug.Log("PathfindingToPlayerActionで経路を求める");
        return State.Success;
    }
}

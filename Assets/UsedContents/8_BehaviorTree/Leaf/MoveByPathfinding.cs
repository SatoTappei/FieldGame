using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索の結果を用いて移動するアクションノード
/// </summary>
public class MoveByPathfinding : BehaviorTreeNode
{
    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        return State.Running;
    }
}

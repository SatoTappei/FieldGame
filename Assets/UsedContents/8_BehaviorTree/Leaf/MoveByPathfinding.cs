using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T���̌��ʂ�p���Ĉړ�����A�N�V�����m�[�h
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

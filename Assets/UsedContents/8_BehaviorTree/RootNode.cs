using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���[�g�m�[�h�̃N���X
/// </summary>
public class RootNode : BehaviorTreeNode
{
    public BehaviorTreeNode _child;

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        return _child.Update();
    }
}

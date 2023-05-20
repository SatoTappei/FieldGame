using System;

/// <summary>
/// �w�肵�����\�b�h��true�̊ԁA���s����f�R���[�^�[�m�[�h�̃N���X
/// </summary>
public class LoopDecorator : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    Func<bool> _judge;
    BehaviorTreeNode _child;

    public LoopDecorator(Func<bool> judge, string nodeName) : base(nodeName)
    {
        _judge = judge;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        _child.Update();

        if (_judge.Invoke())
        {
            return State.Running;
        }
        else
        {
            return State.Success;
        }
    }

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

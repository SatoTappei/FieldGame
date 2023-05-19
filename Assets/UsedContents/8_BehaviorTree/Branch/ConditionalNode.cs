using System;

/// <summary>
/// コンディショナルノードのクラス
/// 条件によって成功か失敗が選ばれる
/// </summary>
public class ConditionalNode : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;
    Func<bool> _judge;

    public ConditionalNode(Func<bool> judge)
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
        if (_judge.Invoke())
        {
            _child.Update();
            return State.Runnning;
        }
        else
        {
            return State.Failure;
        }
    }

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

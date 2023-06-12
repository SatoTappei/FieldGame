using System;

// 不具合がありそうなので未使用

/// <summary>
/// 指定したメソッドで判定するデコレーターノードのクラス
/// メソッドがtrueなら子を実行してRunningを、falseなら失敗を返す
/// </summary>
public class FuncDecorator : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;
    Func<bool> _judge;

    public FuncDecorator(Func<bool> judge)
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
        // 子の実行結果に関わらずRunningを返してはダメなのでは？
        // 子と同じ結果を返すようにしないと子が失敗もしくは成功を返しても反映されない？
        // 現状はFuncがfalse、つまり条件を満たしていなかったらFailureを返すようになっているのは大丈夫そう？
        if (_judge.Invoke())
        {
            _child.Update();
            return State.Running;
        }
        else
        {
            return State.Failure;
        }
    }

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

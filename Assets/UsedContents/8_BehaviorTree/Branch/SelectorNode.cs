using System.Collections.Generic;
using System.Linq;

/// <summary>
/// セレクターノードのクラス
/// 先頭から順に、もしくはランダムに選択する
/// </summary>
public class SelectorNode : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    /// <summary>
    /// 子ノードを選択する際のルール
    /// </summary>
    public enum Rule
    {
        Order,
        Random,
    }

    List<BehaviorTreeNode> _childList = new();
    Rule _rule;

    public SelectorNode(Rule rule, string nodeName) : base(nodeName)
    {
        _rule = rule;
    }

    protected override void OnEnter()
    {
        // ランダムに子を選択する場合は一度シャッフルする
        if (_rule == Rule.Random)
        {
            // TODO:毎フレームToList()を呼んでいるのと同じなので直す
            _childList = _childList.OrderBy(_ => System.Guid.NewGuid()).ToList();
        }
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        foreach(BehaviorTreeNode node in _childList)
        {
            State result = node.Update();

            // 子が失敗した場合は次の子を実行する
            if (result == State.Failure) continue;
            // 子が実行中もしくは成功した場合は、次の子を実行せずに子の結果を返す
            return result;
        }

        return State.Failure;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

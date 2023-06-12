using System.Collections.Generic;
using System.Linq;

public class MonkeyModelSelectorNode : MonkeyModelTreeNode, IMonkeyModelTreeNodeHolder
{
    /// <summary>
    /// 子ノードを選択する際のルール
    /// </summary>
    public enum Rule
    {
        Order,
        Random,
    }

    List<MonkeyModelTreeNode> _childList = new();
    Rule _rule;

    public MonkeyModelSelectorNode(Rule rule, string nodeName) : base(nodeName)
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
        foreach (MonkeyModelTreeNode node in _childList)
        {
            State result = node.Update();

            // 子が失敗した場合は次の子を実行する
            if (result == State.Failure) continue;
            // 子が実行中もしくは成功した場合は、次の子を実行せずに子の結果を返す
            return result;
        }

        return State.Failure;
    }

    public void AddChild(MonkeyModelTreeNode node) => _childList.Add(node);
}

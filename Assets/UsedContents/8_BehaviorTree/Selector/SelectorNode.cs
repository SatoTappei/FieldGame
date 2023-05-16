using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// セレクターノードのクラス
/// 先頭から順に、もしくはランダムに選択する
/// </summary>
public class SelectorNode : BehaviorTreeNode
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
    int _currentChildIndex;

    public SelectorNode(Rule rule)
    {
        _rule = rule;
    }

    protected override void OnEnter()
    {
        // ランダムに子を選択する場合は一度シャッフルする
        if (_rule == Rule.Random)
        {
            _childList = _childList.OrderBy(_ => System.Guid.NewGuid()).ToList();
        }

        _currentChildIndex = 0;
        Debug.Log("Selector開始");
    }

    protected override void OnExit()
    {
        Debug.Log("Selector終了");
    }

    protected override State OnStay()
    {
        State result = _childList[_currentChildIndex].Update();

        if (result == State.Success)
        {
            return State.Success;
        }
        else if (result == State.Failure)
        {
            if(_currentChildIndex == _childList.Count - 1)
            {
                // 最後の子が失敗を返したらSelector自体が失敗を返す
                return State.Failure;
            }
            else
            {
                _currentChildIndex++;
            }
        }

        return State.Runnning;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

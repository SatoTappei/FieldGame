using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シーケンスノードのクラス
/// </summary>
public class SequenceNode : BehaviorTreeNode
{
    List<BehaviorTreeNode> _childList = new();
    int _currentChildIndex;

    protected override void OnEnter()
    {
        _currentChildIndex = 0;
        Debug.Log("Sequence開始");
    }

    protected override void OnExit()
    {
        Debug.Log("Sequence終了");
    }

    protected override State OnStay()
    {
        State result = _childList[_currentChildIndex].Update();

        if (result == State.Success)
        {
            if (_currentChildIndex == _childList.Count - 1)
            {
                return State.Success;
            }
            else
            {
                _currentChildIndex++;
            }
        }
        else if(result == State.Failure)
        {
            // 子が失敗を返したらSequence自体が失敗を返す
            return State.Failure;
        }

        return State.Runnning;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

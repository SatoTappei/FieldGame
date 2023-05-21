using System.Collections.Generic;

/// <summary>
/// シーケンスノードのクラス
/// </summary>
public class SequenceNode : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    List<BehaviorTreeNode> _childList = new();
    /// <summary>
    /// 子が実行中の状態を返した場合に次シーケンスが実行される際
    /// 1番目の子から実行されるのを防ぐために実行中の子の添え字を保持しておく
    /// </summary>
    int _currentChildIndex;

    public SequenceNode(string nodeName) : base(nodeName) { }

    protected override void OnEnter()
    {
        _currentChildIndex = 0;
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        while (_currentChildIndex < _childList.Count)
        {
            State result = _childList[_currentChildIndex].Update();

            // 子が成功した場合は
            if(result == State.Success)
            {
                _currentChildIndex++;
                continue;
            }

            return result;
        }

        return State.Success;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

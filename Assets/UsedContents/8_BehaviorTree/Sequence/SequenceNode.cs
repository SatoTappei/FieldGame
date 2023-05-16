using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V�[�P���X�m�[�h�̃N���X
/// </summary>
public class SequenceNode : BehaviorTreeNode
{
    List<BehaviorTreeNode> _childList = new();
    int _currentChildIndex;

    protected override void OnEnter()
    {
        _currentChildIndex = 0;
        Debug.Log("Sequence�J�n");
    }

    protected override void OnExit()
    {
        Debug.Log("Sequence�I��");
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
            // �q�����s��Ԃ�����Sequence���̂����s��Ԃ�
            return State.Failure;
        }

        return State.Runnning;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

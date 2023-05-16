using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// �Z���N�^�[�m�[�h�̃N���X
/// �擪���珇�ɁA�������̓����_���ɑI������
/// </summary>
public class SelectorNode : BehaviorTreeNode
{
    /// <summary>
    /// �q�m�[�h��I������ۂ̃��[��
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
        // �����_���Ɏq��I������ꍇ�͈�x�V���b�t������
        if (_rule == Rule.Random)
        {
            _childList = _childList.OrderBy(_ => System.Guid.NewGuid()).ToList();
        }

        _currentChildIndex = 0;
        Debug.Log("Selector�J�n");
    }

    protected override void OnExit()
    {
        Debug.Log("Selector�I��");
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
                // �Ō�̎q�����s��Ԃ�����Selector���̂����s��Ԃ�
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

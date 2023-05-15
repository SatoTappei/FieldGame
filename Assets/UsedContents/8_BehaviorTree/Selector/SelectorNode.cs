using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Z���N�^�[�m�[�h�̃N���X
/// </summary>
public class SelectorNode : BehaviorTreeNode
{
    // �m�[�h�̑I����@
    //  �����_����1�I��
    //  ���ԂɑI��

    // �q�̃m�[�h�̒�����1�����s���Đ���������Success��Ԃ�
    // �I�������q�����s�����ꍇ�͕ʂ̎q�����s����
    // �S�Ă̎q�����s��Ԃ����ꍇ�͎��s��Ԃ�
    // �U���������͉񕜂������́c�݂����ȏꍇ�Ɏg��

    List<BehaviorTreeNode> _childList = new();
    int _currentChildIndex;

    protected override void OnEnter()
    {
        _currentChildIndex = 0;
        Debug.Log("Selector�J�n");
    }

    protected override void OnExit()
    {
        Debug.Log("Selector�I��");
    }

    protected override State OnStay()
    {
        // ���t���[���q�̌��ʂ�Ԃ��Ă��炤
        State result = _childList[_currentChildIndex].Update();

        // �Ō�̎q�����s��Ԃ�����Selector���̂����s��Ԃ�
        if (_currentChildIndex == _childList.Count && result == State.Failure)
        {
            return State.Failure;
        }

        // �q��������Ԃ����玟�̎q�����s����
        if (result == State.Success)
        {
            return State.Success;
        }
        // �q�����s��Ԃ����玟�̎q������
        else if (result == State.Failure)
        {
            _currentChildIndex++;
        }

        return State.Runnning;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

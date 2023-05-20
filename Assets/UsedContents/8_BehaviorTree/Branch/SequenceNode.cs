using System.Collections.Generic;

/// <summary>
/// �V�[�P���X�m�[�h�̃N���X
/// </summary>
public class SequenceNode : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    List<BehaviorTreeNode> _childList = new();
    /// <summary>
    /// �q�����s���̏�Ԃ�Ԃ����ꍇ�Ɏ��V�[�P���X�����s������
    /// 1�Ԗڂ̎q������s�����̂�h�����߂Ɏ��s���̎q�̓Y������ێ����Ă���
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

            // �q�����������ꍇ��
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

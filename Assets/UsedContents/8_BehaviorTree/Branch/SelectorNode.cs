using System.Collections.Generic;
using System.Linq;

/// <summary>
/// �Z���N�^�[�m�[�h�̃N���X
/// �擪���珇�ɁA�������̓����_���ɑI������
/// </summary>
public class SelectorNode : BehaviorTreeNode, IBehaviorTreeNodeHolder
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

    public SelectorNode(Rule rule, string nodeName) : base(nodeName)
    {
        _rule = rule;
    }

    protected override void OnEnter()
    {
        // �����_���Ɏq��I������ꍇ�͈�x�V���b�t������
        if (_rule == Rule.Random)
        {
            // TODO:���t���[��ToList()���Ă�ł���̂Ɠ����Ȃ̂Œ���
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

            // �q�����s�����ꍇ�͎��̎q�����s����
            if (result == State.Failure) continue;
            // �q�����s���������͐��������ꍇ�́A���̎q�����s�����Ɏq�̌��ʂ�Ԃ�
            return result;
        }

        return State.Failure;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

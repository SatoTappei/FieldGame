using System.Collections.Generic;
using System.Linq;

public class MonkeyModelSelectorNode : MonkeyModelTreeNode, IMonkeyModelTreeNodeHolder
{
    /// <summary>
    /// �q�m�[�h��I������ۂ̃��[��
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
        foreach (MonkeyModelTreeNode node in _childList)
        {
            State result = node.Update();

            // �q�����s�����ꍇ�͎��̎q�����s����
            if (result == State.Failure) continue;
            // �q�����s���������͐��������ꍇ�́A���̎q�����s�����Ɏq�̌��ʂ�Ԃ�
            return result;
        }

        return State.Failure;
    }

    public void AddChild(MonkeyModelTreeNode node) => _childList.Add(node);
}

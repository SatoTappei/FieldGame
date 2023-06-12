using System;

// �s������肻���Ȃ̂Ŗ��g�p

/// <summary>
/// �w�肵�����\�b�h�Ŕ��肷��f�R���[�^�[�m�[�h�̃N���X
/// ���\�b�h��true�Ȃ�q�����s����Running���Afalse�Ȃ玸�s��Ԃ�
/// </summary>
public class FuncDecorator : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;
    Func<bool> _judge;

    public FuncDecorator(Func<bool> judge)
    {
        _judge = judge;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // �q�̎��s���ʂɊւ�炸Running��Ԃ��Ă̓_���Ȃ̂ł́H
        // �q�Ɠ������ʂ�Ԃ��悤�ɂ��Ȃ��Ǝq�����s�������͐�����Ԃ��Ă����f����Ȃ��H
        // �����Func��false�A�܂�����𖞂����Ă��Ȃ�������Failure��Ԃ��悤�ɂȂ��Ă���̂͑��v�����H
        if (_judge.Invoke())
        {
            _child.Update();
            return State.Running;
        }
        else
        {
            return State.Failure;
        }
    }

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

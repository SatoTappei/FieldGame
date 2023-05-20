using UnityEngine.Events;

// ���X�̏������������ɏ��������̂Ŗ��g�p

/// <summary>
/// ���\�b�h�����s���邾���̃A�N�V�����m�[�h
/// </summary>
public class ExecuteMethodAction : BehaviorTreeNode
{
    UnityAction _action;

    public ExecuteMethodAction(UnityAction action, string nodeName) : base(nodeName)
    {
        _action = action;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        _action?.Invoke();
        return State.Success;
    }
}

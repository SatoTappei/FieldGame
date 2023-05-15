/// <summary>
/// BehaviorTree�Ŏg�p����m�[�h�̃N���X
/// </summary>
public abstract class BehaviorTreeNode
{
    public enum State
    {
        Runnning,
        Failure,
        Success,
    }

    public State CurrentState { get; set; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// Tree�̃N���X����Update()�̃^�C�~���O�ŌĂ΂�郁�\�b�h
    /// �ŏ���1���"OnEnter()+OnStay()"���Ă΂��B
    /// OnStay()��Running�ȊO��Ԃ����ꍇ��"OnStay()+OnExit()"
    /// </summary>
    public State Update()
    {
        if (!IsActive)
        {
            IsActive = true;
            OnEnter();
        }

        CurrentState = OnStay();

        if (CurrentState == State.Failure || CurrentState == State.Success)
        {
            OnExit();
            IsActive = false;
        }

        return CurrentState;
    }

    protected abstract void OnEnter();
    protected abstract State OnStay();
    protected abstract void OnExit();
}
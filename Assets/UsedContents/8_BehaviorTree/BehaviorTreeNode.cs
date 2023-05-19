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

    State _currentState;
    bool _isActive;

    /// <summary>
    /// Tree�̃N���X����Update()�̃^�C�~���O�ŌĂ΂�郁�\�b�h
    /// �ŏ���1���"OnEnter()��OnStay()"���Ă΂��
    /// OnStay()��Running�ȊO��Ԃ����ꍇ��"OnStay()��OnExit()"���Ă΂��
    /// </summary>
    public State Update()
    {
        if (!_isActive)
        {
            _isActive = true;
            OnEnter();
        }

        _currentState = OnStay();

        if (_currentState == State.Failure || _currentState == State.Success)
        {
            OnExit();
            _isActive = false;
        }

        return _currentState;
    }

    protected abstract void OnEnter();
    protected abstract State OnStay();
    protected abstract void OnExit();
}
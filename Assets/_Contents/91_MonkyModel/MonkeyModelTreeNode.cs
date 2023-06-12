using UnityEngine.Events;

public abstract class MonkeyModelTreeNode
{
    public enum State
    {
        Running,
        Failure,
        Success,
    }

    State _currentState;
    bool _isActive;

    public MonkeyModelTreeNode(string nodeName, MonkeyModelBlackBoard blackBoard = null)
    {
        NodeName = nodeName;
        BlackBoard = blackBoard;
    }

    /// <summary>
    /// �e�m�[�h����l��ǂݏ������邽�߂̍���
    /// �g�p����ꍇ�̓R���X�g���N�^�ŎQ�Ƃ�n������
    /// </summary>
    protected MonkeyModelBlackBoard BlackBoard { get; }
    /// <summary>
    /// ���O��UI���ɕ\�����邽�߂̃m�[�h��
    /// </summary>
    public string NodeName { get; }

    public UnityEvent OnNodeEnter;
    public UnityEvent OnNodeExit;

    /// <summary>
    /// Tree�̃N���X����Update()�̃^�C�~���O�ŌĂ΂�郁�\�b�h
    /// �ŏ���1���"OnEnter()��OnStay()"���Ă΂��
    /// OnStay()��Running�ȊO��Ԃ����ꍇ��"OnStay()��OnExit()"���Ă΂��
    /// </summary>
    public State Update()
    {
        if (!_isActive)
        {
            OnNodeEnter?.Invoke();
            _isActive = true;
            OnEnter();
        }

#if UNITY_EDITOR
        //Debug.Log(NodeName + "�����s��");
#endif

        _currentState = OnStay();

        if (_currentState == State.Failure || _currentState == State.Success)
        {
            OnExit();
            _isActive = false;
            OnNodeExit?.Invoke();
        }

        return _currentState;
    }

    protected abstract void OnEnter();
    protected abstract State OnStay();
    protected abstract void OnExit();
}

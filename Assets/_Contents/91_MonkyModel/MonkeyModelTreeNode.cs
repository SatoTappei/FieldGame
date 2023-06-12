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
    /// 各ノードから値を読み書きするための黒板
    /// 使用する場合はコンストラクタで参照を渡すこと
    /// </summary>
    protected MonkeyModelBlackBoard BlackBoard { get; }
    /// <summary>
    /// ログやUI等に表示するためのノード名
    /// </summary>
    public string NodeName { get; }

    public UnityEvent OnNodeEnter;
    public UnityEvent OnNodeExit;

    /// <summary>
    /// TreeのクラスからUpdate()のタイミングで呼ばれるメソッド
    /// 最初の1回は"OnEnter()とOnStay()"が呼ばれる
    /// OnStay()がRunning以外を返した場合は"OnStay()とOnExit()"が呼ばれる
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
        //Debug.Log(NodeName + "を実行中");
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

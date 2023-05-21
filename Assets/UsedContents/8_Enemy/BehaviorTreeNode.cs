using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// BehaviorTreeで使用するノードのクラス
/// </summary>
public abstract class BehaviorTreeNode
{
    public enum State
    {
        Running,
        Failure,
        Success,
    }

    State _currentState;
    bool _isActive;

    public BehaviorTreeNode() { }
    public BehaviorTreeNode(string nodeName, BehaviorTreeBlackBoard blackBoard = null)
    {
        NodeName = nodeName;
        BlackBoard = blackBoard;
    }

    /// <summary>
    /// 各ノードから値を読み書きするための黒板
    /// 使用する場合はコンストラクタで参照を渡すこと
    /// </summary>
    protected BehaviorTreeBlackBoard BlackBoard { get; }
    /// <summary>
    /// ログやUI等に表示するためのノード名
    /// </summary>
    public string NodeName { get; }

    public UnityAction OnNodeEnter { get; set; }
    public UnityAction OnNodeExit { get; set; }

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
        Debug.Log(NodeName + "を実行中");
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
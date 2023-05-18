/// <summary>
/// BehaviorTreeで使用するノードのクラス
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
    /// TreeのクラスからUpdate()のタイミングで呼ばれるメソッド
    /// 最初の1回は"OnEnter()とOnStay()"が呼ばれる
    /// OnStay()がRunning以外を返した場合は"OnStay()とOnExit()"が呼ばれる
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
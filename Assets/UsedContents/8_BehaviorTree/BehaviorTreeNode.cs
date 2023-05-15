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

    public State CurrentState { get; set; }
    public bool IsActive { get; private set; }

    /// <summary>
    /// TreeのクラスからUpdate()のタイミングで呼ばれるメソッド
    /// 最初の1回は"OnEnter()+OnStay()"が呼ばれる。
    /// OnStay()がRunning以外を返した場合は"OnStay()+OnExit()"
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
using UnityEngine;

/// <summary>
/// 任意の時間待機するアクションノード
/// </summary>
public class WaitTimerAction : BehaviorTreeNode
{
    float _waitTime;
    float _time;

    public WaitTimerAction(float waitTime)
    {
        _waitTime = waitTime;
    }

    protected override void OnEnter()
    {
        _time = _waitTime;
    }

    protected override void OnExit()
    {

    }

    protected override State OnStay()
    {
        _time -= Time.deltaTime;
        if (_time < 0)
        {
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}

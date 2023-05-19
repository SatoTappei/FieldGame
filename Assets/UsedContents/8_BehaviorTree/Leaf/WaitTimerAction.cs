using UnityEngine;

/// <summary>
/// �C�ӂ̎��ԑҋ@����A�N�V�����m�[�h
/// </summary>
public class WaitTimerAction : BehaviorTreeNode
{
    float _waitTime;
    float _current;

    public WaitTimerAction(float waitTime)
    {
        _waitTime = waitTime;
    }

    protected override void OnEnter()
    {
        _current = _waitTime;
    }

    protected override void OnExit()
    {

    }

    protected override State OnStay()
    {
        _current -= Time.deltaTime;
        if (_current < 0)
        {
            return State.Success;
        }
        else
        {
            return State.Runnning;
        }
    }
}

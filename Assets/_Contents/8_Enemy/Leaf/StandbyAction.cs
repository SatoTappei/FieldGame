using UnityEngine;

/// <summary>
/// �C�ӂ̎��ԑҋ@����A�N�V�����m�[�h
/// </summary>
public class StandbyAction : BehaviorTreeNode
{
    float _waitTime;
    float _time;

    public StandbyAction(float waitTime, string nodeName) : base(nodeName)
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

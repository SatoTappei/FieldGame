using UnityEngine;

public class MonkeyModelStandbyAction : MonkeyModelTreeNode
{
    float _waitTime;
    float _time;

    public MonkeyModelStandbyAction(float waitTime, string nodeName) : base(nodeName)
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

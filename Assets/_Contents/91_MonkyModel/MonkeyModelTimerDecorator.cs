using UnityEngine;

public class MonkeyModelTimerDecorator : MonkeyModelTreeNode, IMonkeyModelTreeNodeHolder
{
    MonkeyModelTreeNode _child;
    State _waitingState;
    float _interval;
    float _time;

    public MonkeyModelTimerDecorator(State waitingState, float interval, string nodeName) : base(nodeName)
    {
        _waitingState = waitingState;
        _interval = interval;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        _time += Time.deltaTime;
        if (_time > _interval)
        {
            _time = 0;
            return _child.Update();
        }
        else
        {
            return _waitingState;
        }
    }

    public void AddChild(MonkeyModelTreeNode node) => _child = node;
}

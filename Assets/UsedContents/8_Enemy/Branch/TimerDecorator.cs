using UnityEngine;

/// <summary>
/// 一定間隔で子を実行して子と同じ結果を返するデコレーターノードのクラス
/// 待機中は指定した状態を返す
/// </summary>
public class TimerDecorator : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;
    State _waitingState;
    float _interval;
    float _time;

    public TimerDecorator(State waitingState, float interval, string nodeName) : base(nodeName)
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

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

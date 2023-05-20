using UnityEngine;

/// <summary>
/// 指定したメソッドで判定するデコレーターノードのクラス
/// 一定間隔で子を実行して子と同じ結果を返す
/// </summary>
public class TimerDecorator : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;
    float _interval;
    float _time;

    public TimerDecorator(float interval, string nodeName) : base(nodeName)
    {
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
            return State.Failure;
        }
    }

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

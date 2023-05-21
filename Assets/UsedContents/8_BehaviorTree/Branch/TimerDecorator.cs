using UnityEngine;

/// <summary>
/// ���Ԋu�Ŏq�����s���Ďq�Ɠ������ʂ�Ԃ���f�R���[�^�[�m�[�h�̃N���X
/// �ҋ@���͎w�肵����Ԃ�Ԃ�
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

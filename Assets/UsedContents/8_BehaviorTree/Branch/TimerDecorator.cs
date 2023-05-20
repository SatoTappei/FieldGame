using UnityEngine;

/// <summary>
/// �w�肵�����\�b�h�Ŕ��肷��f�R���[�^�[�m�[�h�̃N���X
/// ���Ԋu�Ŏq�����s���Ďq�Ɠ������ʂ�Ԃ�
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

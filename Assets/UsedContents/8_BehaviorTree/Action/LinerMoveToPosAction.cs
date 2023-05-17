using UnityEngine;

/// <summary>
/// �w�肵�����W�Ɍ����Ĉړ�����A�N�V�����m�[�h
/// �ǂ�G�𖳎����Ē����ړ����s��
/// </summary>
public class LinerMoveToPosAction : BehaviorTreeNode
{
    /// <summary>
    /// �ړI�n�ɓ��������Ƃ݂Ȃ�����
    /// </summary>
    static readonly float Approximately = 0.05f;

    Transform _transform;
    Vector3 _targetPos;
    float _speed;

    public LinerMoveToPosAction(Collider collider, Transform transform,
        Vector3 targetPos, float speed) : base(collider)
    {
        _transform = transform;
        _targetPos = targetPos;
        _speed = speed;
    }

    protected override void OnEnter()
    {
        Debug.Log(_targetPos + "�ւ̈ړ��J�n");
    }

    protected override void OnExit()
    {
        Debug.Log(_targetPos + "�ւ̈ړ��I��");
    }

    protected override State OnStay()
    {
        if (IsTriggerEnter)
        {
            Debug.Log("�q�b�g!");
        }

        if (Vector3.Distance(_transform.position, _targetPos) <= Approximately)
        {
            return State.Success;
        }
        else
        {
            float speed = Time.deltaTime * _speed;
            _transform.position = Vector3.MoveTowards(_transform.position, _targetPos, speed);

            return State.Runnning;
        }
    }
}

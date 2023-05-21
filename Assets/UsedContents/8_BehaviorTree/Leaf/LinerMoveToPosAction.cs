using UnityEngine;

/// <summary>
/// �w�肵�����W�Ɍ����Ĉړ�����A�N�V�����m�[�h
/// </summary>
public class LinerMoveToPosAction : BehaviorTreeNode
{
    /// <summary>
    /// �ړI�n�ɓ��������Ƃ݂Ȃ�����
    /// ���̒l�͈ړ����x�𑬂������ꍇ�A�������Ȃ��Ƃ����Ȃ�
    /// </summary>
    static readonly float Approximately = 0.3f;

    Rigidbody _rigidbody;
    Transform _transform;
    Transform _model;
    Vector3 _targetPos;
    float _speed;

    // TODO:BlackBoard�Ƀf�[�^���܂Ƃ߂��̂�BlackBoard��n�������ő��v
    public LinerMoveToPosAction(Rigidbody rigidbody, Vector3 targetPos, float speed,
        string nodeName) : base(nodeName)
    {
        _rigidbody = rigidbody;
        _transform = rigidbody.GetComponent<Transform>();
        _model = rigidbody.transform.GetChild(0);
        if (_model.name != "Model")
        {
            Debug.LogError("��]�����邽�߂�Model��1�Ԗڂ̎q�ɂȂ��Ă��Ȃ��A�������͖��O���Ⴄ");
        }
        _targetPos = targetPos;
        _speed = speed;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        if ((_transform.position - _targetPos).sqrMagnitude <= Approximately)
        {
            return State.Success;
        }
        else
        {
            // �ړ�
            Vector3 velo = (_targetPos - _transform.position).normalized * _speed;
            velo.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velo;

            // Y������]���Ƃ�����]
            Quaternion rot = Quaternion.LookRotation(velo, Vector3.up);
            //_model <- �ړ������ɉ�]������


            return State.Running;
        }
    }
}

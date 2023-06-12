using UnityEngine;

public class MonkeyModelMoveAction : MonkeyModelTreeNode
{
    /// <summary>
    /// �ړI�n�ɓ��������Ƃ݂Ȃ�����
    /// ���̒l�͈ړ����x�𑬂������ꍇ�A�������Ȃ��Ƃ����Ȃ�
    /// </summary>
    static readonly float Approximately = 0.3f;
    /// <summary>
    /// ���炩�̗��R�ňړ����o���Ȃ��Ȃ����ꍇ�ɋ����I�Ɉړ����L�����Z�����邽�߂̎��Ԑ���
    /// �����I�ɐ�����Ԃ�
    /// </summary>
    static readonly float TimeLimit = 12.0f;

    float _playerDetectTimer;
    float _timeUpTimer;

    public MonkeyModelMoveAction(string nodeName, MonkeyModelBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
        _playerDetectTimer = 0;
        _timeUpTimer = 0;
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // ���Ԋu�ōU���J�n�͈͂Ƀv���C���[�����邩���m
        _playerDetectTimer += Time.deltaTime;
        if (_playerDetectTimer > BlackBoard.PlayerDetectInterval)
        {
            _playerDetectTimer = 0;
            if (IsPlayerWithInFireRadius()) return State.Success;
        }
        // ���Ԑ����ŋ����I�Ɉړ����L�����Z��
        _timeUpTimer += Time.deltaTime;
        if (_timeUpTimer > TimeLimit)
        {
            if (BlackBoard.DebugLog)
            {
                Debug.LogWarning("���Ԑ؂�ňړ��������I�ɃL�����Z��: " + BlackBoard.Transform.name);
            }

            return State.Success;
        }

        // �ړ�
        if (IsArrivalTargetPos())
        {
            return State.Success;
        }
        else
        {
            MoveAndRotate();
        }

        return State.Running;
    }

    /// <summary>
    /// �U���J�n�͈͂Ɠ����������A��Q���ƃv���C���[�ɂ����q�b�g���Ȃ�Ray���΂�
    /// ����Ray���q�b�g�����ꍇ�̓v���C���[�����F���Ă���
    /// </summary>
    bool IsPlayerWithInFireRadius()
    {
        Vector3 rayOrigin = BlackBoard.Transform.position;
        rayOrigin.y += BlackBoard.PlayerVisibleRayOffset;
        Vector3 rayDir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;
        rayDir.y = 0;
        bool rayHit = Physics.Raycast(rayOrigin, rayDir, out RaycastHit hit,
            BlackBoard.FireRadius, BlackBoard.PlayerDetectLayer);

        if (rayHit)
        {
            return hit.collider.CompareTag(BlackBoard.PlayerTag);
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// �O���b�h���v���C���[�̍�������ɕ~�����̂ŁAY���W��0�ɂ��Ĕ�r���邱�Ƃ�
    /// ���፷������ꍇ�ł������̔�r���o����
    /// </summary>
    bool IsArrivalTargetPos()
    {
        Vector3 tp = new Vector3(BlackBoard.Player.position.x, 0, BlackBoard.Player.position.z);
        Vector3 btp = new Vector3(BlackBoard.Transform.position.x, 0, BlackBoard.Transform.position.z);
        return (tp - btp).sqrMagnitude <= Approximately;
    }

    void MoveAndRotate()
    {
        Vector3 dir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;

        // �ړ�
        Vector3 velo = dir * BlackBoard.MoveSpeed;
        velo.y = BlackBoard.Rigidbody.velocity.y;
        BlackBoard.Rigidbody.velocity = velo;

        // ��]
        Quaternion playerRot = Quaternion.AngleAxis(BlackBoard.Player.position.y, Vector3.up);
        Quaternion rot = Quaternion.LookRotation(playerRot * dir, Vector3.up);
        rot.x = 0;
        rot.z = 0;
        BlackBoard.Model.rotation = Quaternion.Lerp(BlackBoard.Model.rotation, rot,
            Time.deltaTime * BlackBoard.RotSpeed);
    }
}

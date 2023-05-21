using UnityEngine;
using System.Buffers;

/// <summary>
/// �o�H�T���̌��ʂ�p���Ĉړ�����A�N�V�����m�[�h
/// </summary>
public class MoveByPathfindingAction : BehaviorTreeNode
{
    /// <summary>
    /// �ړ����Ƀv���C���[�����F���邽�߂�Ray��������
    /// Model�̍�����1�Ƃ��Ēl��ݒ肷��
    /// </summary>
    public static readonly float PlayerVisibleRayOffset = 0.5f;
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

    Vector3 _targetPos;
    float _playerDetectTimer;
    float _timeUpTimer;

    public MoveByPathfindingAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
        _targetPos = BlackBoard.Path.Dequeue();
        _playerDetectTimer = 0;
        _timeUpTimer = 0;
    }

    protected override void OnExit()
    {
        // �ړ��������������̓L�����Z�����ꂽ�ꍇ�͍��̌o�H������
        BlackBoard.Path = null;
    }

    protected override State OnStay()
    {
        // ���Ԋu�ōU���J�n�͈͂Ƀv���C���[�����邩���m
        _playerDetectTimer += Time.deltaTime;
        if (_playerDetectTimer > BehaviorTreeBlackBoard.DetectInterval)
        {
            _playerDetectTimer = 0;
            if (IsPlayerWithInFireRadius()) return State.Success;
        }
        // ���Ԑ����ŋ����I�Ɉړ����L�����Z��
        _timeUpTimer += Time.deltaTime;
        if (_timeUpTimer > TimeLimit)
        {
            Debug.LogWarning("���Ԑ؂�ňړ��������I�ɃL�����Z��: " + BlackBoard.Transform.name);
            return State.Success;
        }

        // �ړ�
        if ((_targetPos - BlackBoard.Transform.position).sqrMagnitude <= Approximately)
        {
            // �����ێ����Ă���o�H���玟�̒n�_���擾�o���Ȃ������ꍇ�͈ړ������Ȃ̂Ő�����Ԃ�
            if (!BlackBoard.Path.TryDequeue(out _targetPos)) return State.Success;
        }
        else
        {
            MoveToTarget();
        }

        return State.Running;
    }

    /// <summary>
    /// �U���J�n�͈͂Ɠ����������A��Q���𖳎����ăv���C���[�ɂ����q�b�g���Ȃ�Ray������
    /// ����Ray���q�b�g�����ꍇ�̓v���C���[�����F���Ă���
    /// </summary>
    bool IsPlayerWithInFireRadius()
    {
        RaycastHit[] hits = ArrayPool<RaycastHit>.Shared.Rent(1);
        Vector3 rayOrigin = BlackBoard.Transform.position;
        rayOrigin.y += PlayerVisibleRayOffset;
        Vector3 rayDir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;
        rayDir.y = 0;
        int count = Physics.RaycastNonAlloc(rayOrigin, rayDir, hits, BlackBoard.FireRadius, BlackBoard.PlayerLayer);
        ArrayPool<RaycastHit>.Shared.Return(hits, true);

        return count > 0;
    }

    void MoveToTarget()
    {
        Vector3 velo = (_targetPos - BlackBoard.Transform.position).normalized * BlackBoard.MoveSpeed;
        velo.y = BlackBoard.Rigidbody.velocity.y;
        BlackBoard.Rigidbody.velocity = velo;
    }
}

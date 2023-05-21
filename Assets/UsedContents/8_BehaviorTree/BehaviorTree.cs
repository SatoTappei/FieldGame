using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Rule = SelectorNode.Rule;
using State = BehaviorTreeNode.State;

/// <summary>
/// BehaviorTree�{�̂̃N���X
/// </summary>
public class BehaviorTree : MonoBehaviour
{
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _goalPoint;
    [SerializeField] Transform _item;
    [SerializeField] Transform _item2;
    [SerializeField] BehaviorTreeBlackBoard _blackBoard;

    void Awake()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        SelectorNode ActorStateSelector = new(Rule.Order, "�L�����N�^�[�̏��Selector");
        SequenceNode moveAndAttackSequence = new("�ړ�->�U��Sequence");
        SequenceNode moveSequence = new("�o�H�T��->�ړ�Sequence");
        SequenceNode attackSequence = new("���m->����Sequence");

        LoopDecorator loopDecorator = new("�q���J��Ԃ�");
        TimerDecorator detectPlayerTimer = new(waitingState: State.Failure,
            BehaviorTreeBlackBoard.DetectInterval, "���Ԋu�Ńv���C���[�����mDecorator");
        TimerDecorator fireTimer = new(waitingState: State.Success, 
            _blackBoard.FireRate, "���Ԋu�Ō��m->���˂�����Decorator");

        DetectPlayerAction detectPlayer = new("�v���C���[�����m", _blackBoard);
        PathfindingToPlayerAction pathfindingToPlayer = new("�ˌ��ʒu�܂ł̌o�H�T��", _blackBoard);
        MoveByPathfindingAction moveByPathfindingAction = new("�v���C���[�Ɍ����Ĉړ�", _blackBoard);
        LinerMoveToPosAction standbyAction = new(rigidbody, _item.position, 0.0f, "���̏�őҋ@");
        FireAction fireAction = new("�e�𔭎˂��čU��", _blackBoard);

        // "�ړ�->�U��"or"�ҋ@"��Selector
        ActorStateSelector.AddChild(moveAndAttackSequence);
        ActorStateSelector.AddChild(standbyAction);
        // �ړ�->�U����Sequence
        moveAndAttackSequence.AddChild(moveSequence);
        moveAndAttackSequence.AddChild(loopDecorator);
        // ���Ԋu�Ō��m->�o�H�T��->�ړ���Sequence
        moveSequence.AddChild(detectPlayerTimer);
        moveSequence.AddChild(pathfindingToPlayer);
        moveSequence.AddChild(moveByPathfindingAction);
        detectPlayerTimer.AddChild(detectPlayer);
        // �ړ������������ꍇ�A���Ԋu�Ō��m->�U����Sequence�����s����
        loopDecorator.AddChild(fireTimer);
        fireTimer.AddChild(attackSequence);
        attackSequence.AddChild(detectPlayer);
        attackSequence.AddChild(fireAction);

        RootNode rootNode = new();
        rootNode._child = ActorStateSelector;

        // FixedUpdate()�̃^�C�~���O�Ńm�[�h���X�V����
        this.FixedUpdateAsObservable().Subscribe(_ => rootNode.Update());
    }

    void OnDrawGizmos()
    {
        if (_blackBoard.Transform != null && _blackBoard.Player != null)
        {
            DrawPlayerDetectRange();
            DrawOpenFireRange();
            DrawPlayerVisibleRay();
        }
    }

    void DrawPlayerDetectRange()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_blackBoard.Transform.position, _blackBoard.DetectRadius);
    }

    void DrawOpenFireRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_blackBoard.Transform.position, _blackBoard.FireRadius);
    }

    void DrawPlayerVisibleRay()
    {
        Vector3 rayOrigin = _blackBoard.Transform.position;
        rayOrigin.y += MoveByPathfindingAction.PlayerVisibleRayOffset;
        Vector3 rayDir = (_blackBoard.Player.position - _blackBoard.Transform.position).normalized;
        rayDir.y = 0;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayOrigin, rayDir * _blackBoard.FireRadius);
    }
}

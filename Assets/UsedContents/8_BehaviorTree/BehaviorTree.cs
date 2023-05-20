using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Rule = SelectorNode.Rule;

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

        LoopDecorator loopDecorator = new(Judge, "�q���J��Ԃ�");
        TimerDecorator detectPlayerTimer = new(BehaviorTreeBlackBoard.DetectInterval, "���Ԋu�Ńv���C���[�����mDecorator");
        TimerDecorator fireTimer = new(_blackBoard.FireRate, "���Ԋu�Ō��m->���˂�����Decorator");

        DetectPlayerAction detectPlayer = new("�v���C���[�����m", _blackBoard);
        PathfindingToPlayerAction pathfindingToFirePos = new("�ˌ��ʒu�܂ł̌o�H�T��");
        LinerMoveToPosAction moveToFirePosAction = new(rigidbody, _item.position, _blackBoard.MoveSpeed, "�ˌ��ʒu�܂ňړ�");
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
        moveSequence.AddChild(pathfindingToFirePos);
        moveSequence.AddChild(moveToFirePosAction);
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

    // �����𖞂����܂�Loop�̏��������A��U�����ɒu��
    bool Judge()
    {
        return true;
    }

    void OnDrawGizmos()
    {
        if(_blackBoard.Transform != null)
        {
            // �v���C���[�����m����͈�
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_blackBoard.Transform.position, _blackBoard.DetectRadius);
        }
    }
}

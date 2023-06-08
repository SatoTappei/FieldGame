using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Rule = SelectorNode.Rule;
using State = BehaviorTreeNode.State;
using AnimType = EnemyAnimationModule.AnimType;

/// <summary>
/// BehaviorTree�{�̂̃N���X
/// </summary>
public class BehaviorTree : MonoBehaviour
{
    [SerializeField] BehaviorTreeBlackBoard _blackBoard;
    [SerializeField] EnemyAnimationModule _animationModule;
    [SerializeField] EnemyLifePointModule _lifePointModule;
    [SerializeField] EnemyPerformanceModule _performanceModule;
    [SerializeField] EnemyLineModule _lineModule;

    void Awake()
    {
        // Tree���쐬����
        RootNode rootNode = new();
        rootNode.AddChild(CreateTree());

        // FixedUpdate()�̃^�C�~���O�Ńm�[�h���X�V����
        this.FixedUpdateAsObservable()
            .TakeWhile(_ => _blackBoard.LifePoint > 0)
            .DoOnCompleted(() => 
            {
                // Play���[�h�I�����ɔ��s�����OnCompleted()��h��
                if (_blackBoard.LifePoint > 0) return;
                // ���j���ꂽ�ꍇ�͔�\���ɂ��Ď��S���o���s��
                gameObject.SetActive(false);
                GameManager.Instance.AudioModule.PlaySE(AudioType.SE_Explosion);
                _performanceModule.Defeated(transform.position);
                _lineModule.SendDefeatedLineMessage();
            })
            .Subscribe(_ => rootNode.Update());

        // ���b�Z�[�W����M���đ̗͂����炷Module�ƃA�j���[�V������Module����������A
        // �_���[�W���󂯂��ۂɃA�j���[�V�������Đ����鏈�����R�[���o�b�N�ɓo�^
        _animationModule.InitOnAwake();
        _lifePointModule.InitOnAwake(transform, _blackBoard);
        _lifePointModule.OnDamaged += () => _animationModule.Play(AnimType.Damaged);
        this.OnDisableAsObservable().Subscribe(_ =>
        {
            _lifePointModule.OnDamaged -= () => _animationModule.Play(AnimType.Damaged);
        });
    }

    BehaviorTreeNode CreateTree()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        SelectorNode actorStateSelector = new(Rule.Order, "�L�����N�^�[�̏��Selector");
        SequenceNode moveAndAttackSequence = new("�ړ�->�U��Sequence");
        SequenceNode moveSequence = new("�o�H�T��->�ړ�Sequence");
        SequenceNode attackSequence = new("���m->����Sequence");
        SequenceNode rotSequence = new("��]->�^�C�}�[Sequence");

        LoopDecorator loopDecorator = new("�q���J��Ԃ�");
        TimerDecorator detectPlayerTimer = new(waitingState: State.Failure,
            _blackBoard.PlayerDetectInterval, "���Ԋu�Ńv���C���[�����mDecorator");
        TimerDecorator fireTimer = new(waitingState: State.Success,
            _blackBoard.FireRate, "���Ԋu�Ō��m->���˂�����Decorator");

        DetectPlayerAction detectPlayer = new("�v���C���[�����m", _blackBoard);
        PathfindingToPlayerAction pathfindingToPlayer = new("�ˌ��ʒu�܂ł̌o�H�T��", _blackBoard);
        MoveByPathfindingAction moveByPathfindingAction = new("�v���C���[�Ɍ����Ĉړ�", _blackBoard);
        StandbyAction standbyAction = new(0, "���̏�őҋ@");
        FireAction fireAction = new("�e�𔭎˂��čU��", _blackBoard);
        RotateToPlayerAction rotToPlayerAction = new("�v���C���[�Ɍ����ĉ�]", _blackBoard);

        // "�ړ�->�U��"or"�ҋ@"��Selector
        actorStateSelector.AddChild(moveAndAttackSequence);
        actorStateSelector.AddChild(standbyAction);

        // �ړ�->�U����Sequence
        moveAndAttackSequence.AddChild(moveSequence);
        moveAndAttackSequence.AddChild(loopDecorator);

        // ���Ԋu�Ō��m->�o�H�T��->�ړ���Sequence
        moveSequence.AddChild(detectPlayerTimer);
        moveSequence.AddChild(pathfindingToPlayer);
        moveSequence.AddChild(moveByPathfindingAction);
        detectPlayerTimer.AddChild(detectPlayer);

        // �ړ������������ꍇ�A���Ԋu�Ō��m->�U����Sequence�����s����
        loopDecorator.AddChild(rotSequence);
        rotSequence.AddChild(rotToPlayerAction);
        rotSequence.AddChild(fireTimer);
        fireTimer.AddChild(attackSequence);
        attackSequence.AddChild(detectPlayer);
        attackSequence.AddChild(fireAction);

        // �ړ�����ۂɃA�j���[�V��������悤�R�[���o�b�N�֓o�^
        moveByPathfindingAction.OnNodeEnter += () => _animationModule.Play(AnimType.Move);
        moveByPathfindingAction.OnNodeExit += () => _animationModule.Play(AnimType.Idle);
        this.OnDisableAsObservable().Subscribe(_ =>
        {
            moveByPathfindingAction.OnNodeEnter -= () => _animationModule.Play(AnimType.Move);
            moveByPathfindingAction.OnNodeExit -= () => _animationModule.Play(AnimType.Idle);
        });

        // ���[�g�m�[�h�̎q�Ƃ��ēo�^����m�[�h��Ԃ�
        return actorStateSelector;
    }

    void OnDrawGizmos()
    {
        if (_blackBoard.Transform != null && _blackBoard.Player != null)
        {
            DrawPlayerDetectRange();
            DrawPlayerDetectRay();
            DrawOpenFireRange();
            DrawPlayerVisibleInFireRangeRay();
        }
    }

    void DrawPlayerDetectRange()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_blackBoard.Transform.position, _blackBoard.DetectRadius);
    }

    void DrawPlayerDetectRay()
    {
        Gizmos.color = Color.yellow;
        Vector3 rayOrigin = _blackBoard.Transform.position;
        rayOrigin.y += _blackBoard.PlayerVisibleRayOffset;
        Vector3 dir = (_blackBoard.Player.position - _blackBoard.Transform.position).normalized;
        Gizmos.DrawRay(rayOrigin, dir * _blackBoard.DetectRadius);
    }

    void DrawOpenFireRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_blackBoard.Transform.position, _blackBoard.FireRadius);
    }

    void DrawPlayerVisibleInFireRangeRay()
    {
        Vector3 rayOrigin = _blackBoard.Transform.position;
        rayOrigin.y += _blackBoard.PlayerVisibleRayOffset;
        Vector3 rayDir = (_blackBoard.Player.position - _blackBoard.Transform.position).normalized;
        rayDir.y = 0;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(rayOrigin, rayDir * _blackBoard.FireRadius);
    }
}
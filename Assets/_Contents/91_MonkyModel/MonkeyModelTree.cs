using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Rule = MonkeyModelSelectorNode.Rule;
using State = MonkeyModelTreeNode.State;

[RequireComponent(typeof(MonkeyModelBlackBoard))]
[RequireComponent(typeof(MonkeyModelCallbackRegister))]
public class MonkeyModelTree : MonoBehaviour
{
    MonkeyModelBlackBoard _blackBoard;
    MonkeyModelCallbackRegister _callbackRegister;

    bool _isActive = true;

    void Awake()
    {
        _blackBoard = GetComponent<MonkeyModelBlackBoard>();
        _callbackRegister = GetComponent<MonkeyModelCallbackRegister>();

        MonkeyModelRootNode rootNode = new();
        rootNode.AddChild(CreateTree());

        this.FixedUpdateAsObservable()
           .TakeWhile(_ => _isActive)
           .DoOnCompleted(() =>
           {
               // Play���[�h�I�����ɔ��s�����OnCompleted()��h��
               if (_isActive) return;

               _callbackRegister.OnDefeated?.Invoke();
           })
           .Subscribe(_ => rootNode.Update());
    }

    MonkeyModelTreeNode CreateTree()
    {
        MonkeyModelSelectorNode actorStateSelector = new(Rule.Order, "�L�����N�^�[�̏��Selector");
        MonkeyModelSequenceNode moveAndAttackSequence = new("�ړ�->�U��Sequence");
        MonkeyModelSequenceNode moveSequence = new("�o�H�T��->�ړ�Sequence");
        MonkeyModelSequenceNode attackSequence = new("���m->����Sequence");
        MonkeyModelSequenceNode rotSequence = new("��]->�^�C�}�[Sequence");

        MonkeyModelLoopDecorator loopDecorator = new("�q���J��Ԃ�");
        MonkeyModelTimerDecorator detectPlayerTimer = new(waitingState: State.Failure,
            _blackBoard.PlayerDetectInterval, "���Ԋu�Ńv���C���[�����mDecorator");
        MonkeyModelTimerDecorator fireTimer = new(waitingState: State.Success,
            _blackBoard.FireRate, "���Ԋu�Ō��m->���˂�����Decorator");

        MonkeyModelDetectPlayerAction detectPlayer = new("�v���C���[�����m", _blackBoard);
        MonkeyModelMoveAction moveToPlayer = new("�v���C���[�Ɍ����Ĉړ�", _blackBoard);
        MonkeyModelStandbyAction standbyAction = new(0, "���̏�őҋ@");
        MonkeyModelFireAction fireAction = new("�e�𔭎˂��čU��", _blackBoard);
        MonkeyModelRotateToPlayerAction rotToPlayerAction = new("�v���C���[�Ɍ����ĉ�]", _blackBoard);

        // "�ړ�->�U��"or"�ҋ@"��Selector
        actorStateSelector.AddChild(moveAndAttackSequence);
        actorStateSelector.AddChild(standbyAction);

        // �ړ�->�U����Sequence
        moveAndAttackSequence.AddChild(moveSequence);
        moveAndAttackSequence.AddChild(loopDecorator);

        // ���Ԋu�Ō��m->�o�H�T��->�ړ���Sequence
        moveSequence.AddChild(detectPlayerTimer);
        moveSequence.AddChild(moveToPlayer);
        detectPlayerTimer.AddChild(detectPlayer);

        // �ړ������������ꍇ�A���Ԋu�Ō��m->�U����Sequence�����s����
        loopDecorator.AddChild(rotSequence);
        rotSequence.AddChild(rotToPlayerAction);
        rotSequence.AddChild(fireTimer);
        fireTimer.AddChild(attackSequence);
        attackSequence.AddChild(detectPlayer);
        attackSequence.AddChild(fireAction);

        // �ړ�����ۂɃA�j���[�V��������悤�R�[���o�b�N�֓o�^
        moveToPlayer.OnNodeEnter = _callbackRegister.OnMoveEnter;
        moveToPlayer.OnNodeExit = _callbackRegister.OnMoveExit;
        // �U�����ɌĂ΂��R�[���o�b�N��o�^
        fireAction.OnFire = _callbackRegister.OnFire;
        this.OnDisableAsObservable().Subscribe(_ =>
        {
            moveToPlayer.OnNodeEnter = null;
            moveToPlayer.OnNodeExit = null;
            fireAction.OnFire = null;
        });

        // ���[�g�m�[�h�̎q�Ƃ��ēo�^����m�[�h��Ԃ�
        return actorStateSelector;
    }

    /// <summary>
    /// �C�ӂ̃^�C�~���O�ŊO�����炱�̃��\�b�h���ĂԂ��ƂŁA�X�V���~�߂邱�Ƃ��o����
    /// 1�x�~�߂�Ɠ��������Ƃ͏o���Ȃ�
    /// </summary>
    public void Complete() => _isActive = false;

    void OnDrawGizmos()
    {
        if (_blackBoard != null && _blackBoard.Transform != null && _blackBoard.Player != null)
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

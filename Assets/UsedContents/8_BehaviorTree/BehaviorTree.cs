using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using State = BehaviorTreeNode.State;
using Rule = SelectorNode.Rule;

/// <summary>
/// BehaviorTree�{�̂̃N���X
/// </summary>
public class BehaviorTree : MonoBehaviour
{
    [SerializeField] Collider _collider;
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _goalPoint;
    [SerializeField] Transform _item;
    [SerializeField] Transform _item2;

    RootNode _rootNode = new();
    State _treeState = State.Runnning;
    List<BehaviorTreeNode> _nodeList = new();

    void Start()
    {
        SequenceNode sequence = new();

        LinerMoveToPosAction moveToItem = new(_collider, transform, _item.position, 10.0f);
        WaitTimerAction waitTime = new(2.0f);

        SelectorNode selector = new(Rule.Random);
        LinerMoveToPosAction moveToItem2 = new(_collider, transform, _item2.position, 7.0f);
        LinerMoveToPosAction moveToStartPoint = new(_collider, transform, _startPoint.position, 3.0f);
        // �A�C�e���Ɍ������Ĉړ�������A��莞�ԑҋ@����
        sequence.AddChild(moveToItem);
        sequence.AddChild(waitTime);
        // �A�C�e��2�������̓S�[���n�_�Ɉړ�����
        sequence.AddChild(selector);
        selector.AddChild(moveToItem2);
        selector.AddChild(moveToStartPoint);

        // ���[�g�m�[�h�̎q��Sequence���Ԃ牺����
        _rootNode._child = sequence;

        // Update()�̃^�C�~���O�Ńm�[�h���X�V����
        this.UpdateAsObservable().Subscribe(_ => _rootNode.Update());
    }
}

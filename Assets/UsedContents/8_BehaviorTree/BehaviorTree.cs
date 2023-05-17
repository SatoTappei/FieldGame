using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using State = BehaviorTreeNode.State;
using Rule = SelectorNode.Rule;

/// <summary>
/// BehaviorTree本体のクラス
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
        // アイテムに向かって移動した後、一定時間待機する
        sequence.AddChild(moveToItem);
        sequence.AddChild(waitTime);
        // アイテム2もしくはゴール地点に移動する
        sequence.AddChild(selector);
        selector.AddChild(moveToItem2);
        selector.AddChild(moveToStartPoint);

        // ルートノードの子にSequenceをぶら下げる
        _rootNode._child = sequence;

        // Update()のタイミングでノードを更新する
        this.UpdateAsObservable().Subscribe(_ => _rootNode.Update());
    }
}

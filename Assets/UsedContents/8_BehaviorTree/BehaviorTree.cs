using System.Collections.Generic;
using UnityEngine;
using State = BehaviorTreeNode.State;

/// <summary>
/// BehaviorTree本体のクラス
/// </summary>
public class BehaviorTree : MonoBehaviour
{
    // 参考動画で現在のアクションの状態を管理するための列挙型を定義していた
    public enum ActionState 
    {
        Idle,
        Working,
    };
    ActionState _state = ActionState.Idle;

    // TODO:この地点間を行き来するようにしたい
    [SerializeField] Transform _startPoint;
    [SerializeField] Transform _item;

    public RootNode _rootNode = new();
    public State _treeState = State.Runnning;
    public List<BehaviorTreeNode> _nodeList = new();

    void Start()
    {
        // アイテムに向かって移動した後、スタート地点まで戻るSequence
        SequenceNode sequence = new ();
        LinerMoveToPosAction moveToItem = new(transform, _item.position, 10.0f);
        LinerMoveToPosAction moveToStartPoint = new(transform, _startPoint.position, 3.0f);
        sequence.AddChild(moveToItem);
        sequence.AddChild(moveToStartPoint);

        // ルートノードの子にSequenceをぶら下げる
        _rootNode._child = sequence;
    }

    // 参考動画での方針
    // 木を利用するものび側にメソッドを用意してデリゲートで譲渡している。
    // そうすることで各ノードが必要になる参照などの情報が減らせる
    State GoToLocation(Vector3 destination)
    {
        // 現在地点からターゲットまでの距離
        float distance = Vector3.Distance(destination, transform.position);
        
        if (_state == ActionState.Idle)
        {
            // 次の地点へ
            _state = ActionState.Working;
        }
        // else if(ターゲットまでが遠すぎる場合)
        // state = ActionState.Idle;
        // return State.Failure;
        
        // else if(ターゲットまでの距離が一定以下なら)
        // state = ActionState.Idle;
        // return State.Success;

        return State.Runnning;
    }

    // 参考動画ではお金を持っているかどうかの関数
    // bool型ではなくState型で返す
    public State HasMoney()
    {
        return State.Success;
    }

    void Update()
    {
        _rootNode.Update();
    }
}

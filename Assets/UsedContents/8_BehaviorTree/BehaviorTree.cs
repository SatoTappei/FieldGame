using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Rule = SelectorNode.Rule;

/// <summary>
/// BehaviorTree本体のクラス
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

        SelectorNode ActorStateSelector = new(Rule.Order, "キャラクターの状態Selector");
        SequenceNode moveAndAttackSequence = new("移動->攻撃Sequence");
        SequenceNode moveSequence = new("経路探索->移動Sequence");
        SequenceNode attackSequence = new("検知->発射Sequence");

        LoopDecorator loopDecorator = new(Judge, "子を繰り返す");
        TimerDecorator detectPlayerTimer = new(BehaviorTreeBlackBoard.DetectInterval, "一定間隔でプレイヤーを検知Decorator");
        TimerDecorator fireTimer = new(_blackBoard.FireRate, "一定間隔で検知->発射をするDecorator");

        DetectPlayerAction detectPlayer = new("プレイヤーを検知", _blackBoard);
        PathfindingToPlayerAction pathfindingToFirePos = new("射撃位置までの経路探索");
        LinerMoveToPosAction moveToFirePosAction = new(rigidbody, _item.position, _blackBoard.MoveSpeed, "射撃位置まで移動");
        LinerMoveToPosAction standbyAction = new(rigidbody, _item.position, 0.0f, "その場で待機");
        FireAction fireAction = new("弾を発射して攻撃", _blackBoard);

        // "移動->攻撃"or"待機"のSelector
        ActorStateSelector.AddChild(moveAndAttackSequence);
        ActorStateSelector.AddChild(standbyAction);
        // 移動->攻撃のSequence
        moveAndAttackSequence.AddChild(moveSequence);
        moveAndAttackSequence.AddChild(loopDecorator);
        // 一定間隔で検知->経路探索->移動のSequence
        moveSequence.AddChild(detectPlayerTimer);
        moveSequence.AddChild(pathfindingToFirePos);
        moveSequence.AddChild(moveToFirePosAction);
        detectPlayerTimer.AddChild(detectPlayer);
        // 移動が完了した場合、一定間隔で検知->攻撃のSequenceを実行する
        loopDecorator.AddChild(fireTimer);
        fireTimer.AddChild(attackSequence);
        attackSequence.AddChild(detectPlayer);
        attackSequence.AddChild(fireAction);

        RootNode rootNode = new();
        rootNode._child = ActorStateSelector;

        // FixedUpdate()のタイミングでノードを更新する
        this.FixedUpdateAsObservable().Subscribe(_ => rootNode.Update());
    }

    // 条件を満たすまでLoopの条件処理、一旦ここに置く
    bool Judge()
    {
        return true;
    }

    void OnDrawGizmos()
    {
        if(_blackBoard.Transform != null)
        {
            // プレイヤーを検知する範囲
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_blackBoard.Transform.position, _blackBoard.DetectRadius);
        }
    }
}

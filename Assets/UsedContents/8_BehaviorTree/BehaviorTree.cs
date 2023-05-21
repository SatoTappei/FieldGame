using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Rule = SelectorNode.Rule;
using State = BehaviorTreeNode.State;

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

        LoopDecorator loopDecorator = new("子を繰り返す");
        TimerDecorator detectPlayerTimer = new(waitingState: State.Failure,
            BehaviorTreeBlackBoard.DetectInterval, "一定間隔でプレイヤーを検知Decorator");
        TimerDecorator fireTimer = new(waitingState: State.Success, 
            _blackBoard.FireRate, "一定間隔で検知->発射をするDecorator");

        DetectPlayerAction detectPlayer = new("プレイヤーを検知", _blackBoard);
        PathfindingToPlayerAction pathfindingToPlayer = new("射撃位置までの経路探索", _blackBoard);
        MoveByPathfindingAction moveByPathfindingAction = new("プレイヤーに向けて移動", _blackBoard);
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
        moveSequence.AddChild(pathfindingToPlayer);
        moveSequence.AddChild(moveByPathfindingAction);
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

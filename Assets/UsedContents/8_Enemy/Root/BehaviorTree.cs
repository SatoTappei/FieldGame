using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Rule = SelectorNode.Rule;
using State = BehaviorTreeNode.State;
using AnimType = EnemyAnimationModule.AnimType;

/// <summary>
/// BehaviorTree本体のクラス
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
        // Treeを作成する
        RootNode rootNode = new();
        rootNode.AddChild(CreateTree());

        // FixedUpdate()のタイミングでノードを更新する
        this.FixedUpdateAsObservable()
            .TakeWhile(_ => _blackBoard.LifePoint > 0)
            .DoOnCompleted(() => 
            {
                // Playモード終了時に発行されるOnCompleted()を防ぐ
                if (_blackBoard.LifePoint > 0) return;
                // 撃破された場合は非表示にして死亡演出を行う
                gameObject.SetActive(false);
                GameManager.Instance.AudioModule.PlaySE(AudioType.SE_Explosion);
                _performanceModule.Defeated(transform.position);
                _lineModule.SendDefeatedLineMessage();
            })
            .Subscribe(_ => rootNode.Update());

        // メッセージを受信して体力を減らすModuleとアニメーションのModuleを初期化後、
        // ダメージを受けた際にアニメーションを再生する処理をコールバックに登録
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

        SelectorNode actorStateSelector = new(Rule.Order, "キャラクターの状態Selector");
        SequenceNode moveAndAttackSequence = new("移動->攻撃Sequence");
        SequenceNode moveSequence = new("経路探索->移動Sequence");
        SequenceNode attackSequence = new("検知->発射Sequence");
        SequenceNode rotSequence = new("回転->タイマーSequence");

        LoopDecorator loopDecorator = new("子を繰り返す");
        TimerDecorator detectPlayerTimer = new(waitingState: State.Failure,
            _blackBoard.PlayerDetectInterval, "一定間隔でプレイヤーを検知Decorator");
        TimerDecorator fireTimer = new(waitingState: State.Success,
            _blackBoard.FireRate, "一定間隔で検知->発射をするDecorator");

        DetectPlayerAction detectPlayer = new("プレイヤーを検知", _blackBoard);
        PathfindingToPlayerAction pathfindingToPlayer = new("射撃位置までの経路探索", _blackBoard);
        MoveByPathfindingAction moveByPathfindingAction = new("プレイヤーに向けて移動", _blackBoard);
        StandbyAction standbyAction = new(0, "その場で待機");
        FireAction fireAction = new("弾を発射して攻撃", _blackBoard);
        RotateToPlayerAction rotToPlayerAction = new("プレイヤーに向けて回転", _blackBoard);

        // "移動->攻撃"or"待機"のSelector
        actorStateSelector.AddChild(moveAndAttackSequence);
        actorStateSelector.AddChild(standbyAction);

        // 移動->攻撃のSequence
        moveAndAttackSequence.AddChild(moveSequence);
        moveAndAttackSequence.AddChild(loopDecorator);

        // 一定間隔で検知->経路探索->移動のSequence
        moveSequence.AddChild(detectPlayerTimer);
        moveSequence.AddChild(pathfindingToPlayer);
        moveSequence.AddChild(moveByPathfindingAction);
        detectPlayerTimer.AddChild(detectPlayer);

        // 移動が完了した場合、一定間隔で検知->攻撃のSequenceを実行する
        loopDecorator.AddChild(rotSequence);
        rotSequence.AddChild(rotToPlayerAction);
        rotSequence.AddChild(fireTimer);
        fireTimer.AddChild(attackSequence);
        attackSequence.AddChild(detectPlayer);
        attackSequence.AddChild(fireAction);

        // 移動する際にアニメーションするようコールバックへ登録
        moveByPathfindingAction.OnNodeEnter += () => _animationModule.Play(AnimType.Move);
        moveByPathfindingAction.OnNodeExit += () => _animationModule.Play(AnimType.Idle);
        this.OnDisableAsObservable().Subscribe(_ =>
        {
            moveByPathfindingAction.OnNodeEnter -= () => _animationModule.Play(AnimType.Move);
            moveByPathfindingAction.OnNodeExit -= () => _animationModule.Play(AnimType.Idle);
        });

        // ルートノードの子として登録するノードを返す
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
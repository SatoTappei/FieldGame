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
               // Playモード終了時に発行されるOnCompleted()を防ぐ
               if (_isActive) return;

               _callbackRegister.OnDefeated?.Invoke();
           })
           .Subscribe(_ => rootNode.Update());
    }

    MonkeyModelTreeNode CreateTree()
    {
        MonkeyModelSelectorNode actorStateSelector = new(Rule.Order, "キャラクターの状態Selector");
        MonkeyModelSequenceNode moveAndAttackSequence = new("移動->攻撃Sequence");
        MonkeyModelSequenceNode moveSequence = new("経路探索->移動Sequence");
        MonkeyModelSequenceNode attackSequence = new("検知->発射Sequence");
        MonkeyModelSequenceNode rotSequence = new("回転->タイマーSequence");

        MonkeyModelLoopDecorator loopDecorator = new("子を繰り返す");
        MonkeyModelTimerDecorator detectPlayerTimer = new(waitingState: State.Failure,
            _blackBoard.PlayerDetectInterval, "一定間隔でプレイヤーを検知Decorator");
        MonkeyModelTimerDecorator fireTimer = new(waitingState: State.Success,
            _blackBoard.FireRate, "一定間隔で検知->発射をするDecorator");

        MonkeyModelDetectPlayerAction detectPlayer = new("プレイヤーを検知", _blackBoard);
        MonkeyModelMoveAction moveToPlayer = new("プレイヤーに向けて移動", _blackBoard);
        MonkeyModelStandbyAction standbyAction = new(0, "その場で待機");
        MonkeyModelFireAction fireAction = new("弾を発射して攻撃", _blackBoard);
        MonkeyModelRotateToPlayerAction rotToPlayerAction = new("プレイヤーに向けて回転", _blackBoard);

        // "移動->攻撃"or"待機"のSelector
        actorStateSelector.AddChild(moveAndAttackSequence);
        actorStateSelector.AddChild(standbyAction);

        // 移動->攻撃のSequence
        moveAndAttackSequence.AddChild(moveSequence);
        moveAndAttackSequence.AddChild(loopDecorator);

        // 一定間隔で検知->経路探索->移動のSequence
        moveSequence.AddChild(detectPlayerTimer);
        moveSequence.AddChild(moveToPlayer);
        detectPlayerTimer.AddChild(detectPlayer);

        // 移動が完了した場合、一定間隔で検知->攻撃のSequenceを実行する
        loopDecorator.AddChild(rotSequence);
        rotSequence.AddChild(rotToPlayerAction);
        rotSequence.AddChild(fireTimer);
        fireTimer.AddChild(attackSequence);
        attackSequence.AddChild(detectPlayer);
        attackSequence.AddChild(fireAction);

        // 移動する際にアニメーションするようコールバックへ登録
        moveToPlayer.OnNodeEnter = _callbackRegister.OnMoveEnter;
        moveToPlayer.OnNodeExit = _callbackRegister.OnMoveExit;
        // 攻撃時に呼ばれるコールバックを登録
        fireAction.OnFire = _callbackRegister.OnFire;
        this.OnDisableAsObservable().Subscribe(_ =>
        {
            moveToPlayer.OnNodeEnter = null;
            moveToPlayer.OnNodeExit = null;
            fireAction.OnFire = null;
        });

        // ルートノードの子として登録するノードを返す
        return actorStateSelector;
    }

    /// <summary>
    /// 任意のタイミングで外部からこのメソッドを呼ぶことで、更新を止めることが出来る
    /// 1度止めると動かすことは出来ない
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

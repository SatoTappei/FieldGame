using UniRx;
using UniRx.Triggers;

/// <summary>
/// 弾を発射して攻撃を行うアクションノード
/// </summary>
public class FireAction : BehaviorTreeNode
{
    ActorBulletPool _bulletPool;

    public FireAction(string nodeName, BehaviorTreeBlackBoard blackBoard) 
        : base(nodeName, blackBoard)
    {
        // 敵の個体毎に弾を保持しておくためのプールを生成する
        // プールはオブジェクトがDestroyされるタイミングで破棄される
        _bulletPool = new(BlackBoard.Bullet, $"EnemyBulletPool{BlackBoard.Transform.GetInstanceID()}");
        BlackBoard.Transform.OnDestroyAsObservable().Subscribe(_ => _bulletPool.Clear());
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        BlackBoard.FireParticle.Play();
        TriggerByMonoBroker.Instance.AddShootData(ShootData.BulletType.Enemy, 
            BlackBoard.Transform.position, BlackBoard.Model.forward);

        ActorBullet bullet = _bulletPool.Rent();
        bullet.OnRent(BlackBoard.Model, BlackBoard.Model.position + BlackBoard.Model.forward);

        return State.Success;
    }
}

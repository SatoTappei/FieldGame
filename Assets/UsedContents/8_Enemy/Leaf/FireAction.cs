using UniRx;
using UniRx.Triggers;

/// <summary>
/// �e�𔭎˂��čU�����s���A�N�V�����m�[�h
/// </summary>
public class FireAction : BehaviorTreeNode
{
    ActorBulletPool _bulletPool;

    public FireAction(string nodeName, BehaviorTreeBlackBoard blackBoard) 
        : base(nodeName, blackBoard)
    {
        // �G�̌̖��ɒe��ێ����Ă������߂̃v�[���𐶐�����
        // �v�[���̓I�u�W�F�N�g��Destroy�����^�C�~���O�Ŕj�������
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

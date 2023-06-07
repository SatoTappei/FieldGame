using UnityEngine;

/// <summary>
/// 攻撃を行うアクションノード
/// </summary>
public class FireAction : BehaviorTreeNode
{
    public FireAction(string nodeName, BehaviorTreeBlackBoard blackBoard) 
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
        //var v = GameObject.FindGameObjectWithTag()
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        BlackBoard.FireParticle.Play();
        TriggerByMonoBroker.Instance.AddShootData(ShootData.BulletType.Enemy, 
            BlackBoard.Transform.position, BlackBoard.Model.forward);
        
        // コライダーを発射する処理が必要
        // TransformとModelへの参照は黒板にある
        // Poolから弾を取り出して発射したい
        // Poolへの参照どうするか問題
        // このノードは使いまわしたい、コールバックを渡して実行するようにすれば近接攻撃にも対応できる？
        // 案1: このクラスにプールをstaticで持たせる

        // ★優先:攻撃をどうするか？このゲームには遠距離攻撃しか無い事は確定している。近接攻撃は考慮しないで良し

        return State.Success;
    }
}

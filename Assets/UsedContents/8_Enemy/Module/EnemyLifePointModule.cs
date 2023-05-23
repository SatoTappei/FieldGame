using UniRx;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 体力の制御を行うクラス
/// </summary>
[System.Serializable]
public class EnemyLifePointModule
{
    public UnityAction OnDamaged;
    public UnityAction OnDefeated;

    /// <summary>
    /// 弾がヒットしたら黒板の体力の値を書き換える
    /// </summary>
    public void InitOnAwake(Transform transform, BehaviorTreeBlackBoard blackBoard)
    {
        MessageBroker.Default.Receive<DamageData>()
            .Where(data => transform.CompareTag(data.TargetTag))
            .Where(data => (data.HitPos - transform.position).sqrMagnitude < data.Range * data.Range)
            .Where(_ => blackBoard.LifePoint > 0)
            .Subscribe(_ => 
            {
                OnDamaged?.Invoke();
                if (--blackBoard.LifePoint <= 0)
                {
                    OnDefeated?.Invoke();
                }
            }).AddTo(transform);
    }
}
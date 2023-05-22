using UniRx;
using UnityEngine;

/// <summary>
/// 体力の制御を行うクラス
/// </summary>
[System.Serializable]
public class EnemyHealthModule
{
    public void InitOnAwake(Transform transform)
    {
        // 弾が当たった位置との距離がRange以下だった場合は弾がヒットした
        MessageBroker.Default.Receive<DamageData>()
            .Where(data => transform.CompareTag(data.TargetTag))
            .Where(data => (data.HitPos - transform.position).sqrMagnitude < data.Range * data.Range)
            .Subscribe(_ => Debug.Log("ダメージを受けた"))
            .AddTo(transform);
    }
}
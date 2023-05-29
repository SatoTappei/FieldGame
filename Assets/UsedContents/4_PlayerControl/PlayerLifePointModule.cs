using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーの体力を制御するクラス
/// 敵弾がぶつかった際に処理をトリガーする
/// </summary>
[System.Serializable]
public class PlayerLifePointModule
{
    [Header("最大体力(ダメージ1の弾に対して)")]
    [SerializeField] int _maxLifePoint = 3;

    ReactiveProperty<int> _lifePoint = new();

    public IReadOnlyReactiveProperty<int> CurrentLifePoint => _lifePoint;

    public void InitOnAwake(Transform transform)
    {
        Reset();

        MessageBroker.Default.Receive<DamageData>()
            .Where(data => transform.CompareTag(data.Tag))
            .Where(data => (data.HitPos - transform.position).sqrMagnitude < data.Range * data.Range)
            .Where(_ => _lifePoint.Value > 0)
            .Subscribe(_ => 
            {
                // ダメージを受けて体力が減ったメッセージを送信
                MessageBroker.Default.Publish(new PlayerLifePointData(_lifePoint.Value, _lifePoint.Value - 1));
                _lifePoint.Value--;
            }).AddTo(transform);
    }

    /// <summary>
    /// リスポーンする際に体力をリセットする必要がある
    /// </summary>
    void Reset() => _lifePoint.Value = _maxLifePoint;
}
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// プレイヤーの体力を表示するUI
/// </summary>
[System.Serializable]
public class LifePointGaugeModule
{
    [SerializeField] Image[] _icons;
    [Header("アイコンの色の設定(デフォルト/ダメージ)")]
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _damagedColor;
    [Header("プレイヤーの体力と同じ値を設定する")]
    [SerializeField] int _playerMaxLifePoint;

    public void InitOnAwake(GameObject gameObject)
    {
        MessageBroker.Default.Receive<PlayerLifePointData>().Subscribe(data =>
        {
            float value = _icons.Length * (data.AfterValue * 1.0f / _playerMaxLifePoint * 1.0f);
            for (int i = 0; i < _icons.Length; i++)
            {
                _icons[i].color = i < value ? _defaultColor : _damagedColor;
            }
        }).AddTo(gameObject);
    }
}

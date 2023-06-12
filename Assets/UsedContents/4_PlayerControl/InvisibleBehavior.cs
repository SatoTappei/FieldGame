using UnityEngine;

/// <summary>
/// 対象に視認されない状態を制御するクラス
/// </summary>
[System.Serializable]
public class InvisibleBehavior
{
    [Header("透明状態の時に属するレイヤー名")]
    [SerializeField] string _invisibleLayerName = "Invisible";

    LayerMask _defaultLayer;

    public void Active(GameObject gameObject)
    {
        _defaultLayer = LayerMask.NameToLayer(gameObject.layer.ToString());
        gameObject.layer = LayerMask.NameToLayer(_invisibleLayerName);
    }

    public void Inactive(GameObject gameObject) => gameObject.layer = _defaultLayer;
}

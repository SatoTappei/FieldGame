using UnityEngine;

/// <summary>
/// �ΏۂɎ��F����Ȃ���Ԃ𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class InvisibleBehavior
{
    [Header("������Ԃ̎��ɑ����郌�C���[��")]
    [SerializeField] string _invisibleLayerName = "Invisible";

    LayerMask _defaultLayer;

    public void Active(GameObject gameObject)
    {
        _defaultLayer = LayerMask.NameToLayer(gameObject.layer.ToString());
        gameObject.layer = LayerMask.NameToLayer(_invisibleLayerName);
    }

    public void Inactive(GameObject gameObject) => gameObject.layer = _defaultLayer;
}

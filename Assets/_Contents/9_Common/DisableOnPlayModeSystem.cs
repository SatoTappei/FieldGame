using UnityEngine;

/// <summary>
/// Playモードに入った時に非表示にさせるクラス
/// 主にデバッグ用
/// </summary>
public class DisableOnPlayModeSystem : MonoBehaviour
{
    [Header("Rendererだけ非表示になるので注意")]
    [SerializeField] Renderer[] _objects;

    void Start()
    {
        foreach(Renderer v in _objects)
        {
            v.enabled = false;
        }
    }
}

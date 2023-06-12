using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面中央に表示される照準のクラス
/// UI本体にアタッチされ、PlayerAimRaycastModuleから操作される
/// </summary>
public class PlayerAimsight : MonoBehaviour
{
    [SerializeField] Transform _period;
    [SerializeField] Color _enemyOverlapColor;
    [SerializeField] Color _dontOverlapColor;

    Image _image;
    Transform _transform;
    Color _defaultColor;

    void Awake()
    {
        _transform = transform;
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    public void Active() => transform.localScale = Vector3.one;
    public void Inactive() => transform.localScale = Vector3.zero;

    public void SetPos(Vector3 pos) => _transform.position = Camera.main.WorldToScreenPoint(pos);
    public void SetDontOverlapColor() => _image.color = _dontOverlapColor;

    public void SetDefaultColor()
    {
        _image.color = _defaultColor;
        _period.localScale = Vector3.zero;
    }

    public void SetEnemyOverlapColor()
    {
        _image.color = _enemyOverlapColor;
        _period.localScale = Vector3.one;
    }
}

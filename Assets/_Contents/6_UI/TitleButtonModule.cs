using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// タイトルのボタン毎の処理のクラス
/// </summary>
[System.Serializable]
public class TitleButtonModule
{
    [SerializeField] Button _button;
    [SerializeField] Image _cursor;

    /// <summary>
    /// クリック時の処理だけ外部から追加できるように公開する
    /// </summary>
    public event UnityAction OnSubmit;

    public void InitOnStart()
    {
        _button.OnSelectAsObservable().Subscribe(_ => OnSelect());
        _button.OnDeselectAsObservable().Subscribe(_ => OnDeselect());
        _button.onClick.AddListener(Submit);

        _cursor.color = Color.clear;
    }

    /// <summary>
    /// 音を鳴らしてコールバックを実行するだけで
    /// 各種演出を追加したり同期させるようにはなっていない
    /// </summary>
    void Submit()
    {
        OnSubmit.Invoke();
        GameManager.Instance?.AudioModule.PlaySE(AudioType.SE_ButtonSubmit);
    }

    void FadeIn()
    {
    }

    void FadeOut()
    {
    }

    void OnSelect()
    {
        _button.transform.localScale = Vector3.one * 1.1f;
        _button.image.color = _button.colors.selectedColor;
        _cursor.color = Color.white;

        GameManager.Instance?.AudioModule.PlaySE(AudioType.SE_ButtonSelect);
    }

    void OnDeselect()
    {
        _button.transform.localScale = Vector3.one;
        _button.image.color = _button.colors.normalColor;
        _cursor.color = Color.clear;
    }
}

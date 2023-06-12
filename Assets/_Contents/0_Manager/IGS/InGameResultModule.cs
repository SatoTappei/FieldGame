using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 脱出成功後のリザルト画面を制御するクラス
/// </summary>
[System.Serializable]
public class InGameResultModule
{
    /// <summary>
    /// ボタンをクリックした際の演出用の時間、適当
    /// </summary>
    static readonly float OnClickDelay = 0.5f;

    [SerializeField] Transform _resultUIRoot;
    [Header("タイトルに戻るボタン")]
    [SerializeField] Button _resultButton;

    public void ActiveAndSelectResultButton()
    {
        _resultUIRoot.localScale = Vector3.one;
        EventSystem.current.SetSelectedGameObject(_resultButton.gameObject);
    }

    public void Inactive()
    {
        _resultUIRoot.localScale = Vector3.zero;
        EventSystem.current.SetSelectedGameObject(null);
    }

    /// <summary>
    /// タイトルに戻るボタンがクリックされるまで待つ
    /// </summary>
    public async UniTask Execute(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        AsyncUnityEventHandler eventHandler = _resultButton.onClick.GetAsyncEventHandler(token);
        await eventHandler.OnInvokeAsync();

        GameManager.Instance.AudioModule.PlaySE(AudioType.SE_ButtonSubmit);
        await UniTask.Delay(System.TimeSpan.FromSeconds(OnClickDelay), cancellationToken: token);
    }
}

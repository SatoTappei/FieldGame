using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// �E�o������̃��U���g��ʂ𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class InGameResultModule
{
    /// <summary>
    /// �{�^�����N���b�N�����ۂ̉��o�p�̎��ԁA�K��
    /// </summary>
    static readonly float OnClickDelay = 0.5f;

    [SerializeField] Transform _resultUIRoot;
    [Header("�^�C�g���ɖ߂�{�^��")]
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
    /// �^�C�g���ɖ߂�{�^�����N���b�N�����܂ő҂�
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
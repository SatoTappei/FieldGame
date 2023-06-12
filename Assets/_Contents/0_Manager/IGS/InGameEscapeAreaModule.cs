using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �E�o�n�_�𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class InGameEscapeAreaModule
{
    [Header("�E�o�n�_�̃R���C�_�[���K�v")]
    [SerializeField] GameObject _playerEscapeArea;

    public void Active() => _playerEscapeArea.SetActive(true);
    public void Inactive() => _playerEscapeArea.SetActive(false);

    /// <summary>
    /// �E�o�n�_�̃R���C�_�[�Ƀv���C���[���G���܂ő҂�
    /// </summary>
    public async UniTask Execute(CancellationToken token, GameObject gameObject)
    {
        token.ThrowIfCancellationRequested();

        if(!_playerEscapeArea.TryGetComponent(out Collider collider))
        {
            Debug.LogError("�E�o�n�_�̃I�u�W�F�N�g�ɃR���C�_�[���t���Ă��Ȃ�");
            return;
        }

        bool flag = false;
        collider.OnTriggerEnterAsObservable().Where(other => other.CompareTag("Player"))
            .FirstOrDefault().Subscribe(_ => flag = true).AddTo(gameObject);

        await UniTask.WaitUntil(() => flag, cancellationToken: token);
    }
}

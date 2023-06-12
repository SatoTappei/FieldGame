using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 脱出地点を制御するクラス
/// </summary>
[System.Serializable]
public class InGameEscapeAreaModule
{
    [Header("脱出地点のコライダーが必要")]
    [SerializeField] GameObject _playerEscapeArea;

    public void Active() => _playerEscapeArea.SetActive(true);
    public void Inactive() => _playerEscapeArea.SetActive(false);

    /// <summary>
    /// 脱出地点のコライダーにプレイヤーが触れるまで待つ
    /// </summary>
    public async UniTask Execute(CancellationToken token, GameObject gameObject)
    {
        token.ThrowIfCancellationRequested();

        if(!_playerEscapeArea.TryGetComponent(out Collider collider))
        {
            Debug.LogError("脱出地点のオブジェクトにコライダーが付いていない");
            return;
        }

        bool flag = false;
        collider.OnTriggerEnterAsObservable().Where(other => other.CompareTag("Player"))
            .FirstOrDefault().Subscribe(_ => flag = true).AddTo(gameObject);

        await UniTask.WaitUntil(() => flag, cancellationToken: token);
    }
}

using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// プレイヤーが死んだ際の処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerDefeatedBehavior
{
    [SerializeField] Transform _respawnPoint;
    [Header("死亡時にプレイヤーの入力を無効化する時間")]
    [SerializeField] float _inputDisableTime = 1.5f;

    public void Respawn(Transform transform)
    {
        transform.position = _respawnPoint.position;
    }

    /// <summary>
    /// 非同期処理で一定時間プレイヤーの入力を止めているので
    /// Update()でフレーム毎にdeltaTimeで更新しているものと噛み合っていないので注意
    /// </summary>
    public async UniTaskVoid PlayerInputDisableDelayedEnableAsync(CancellationToken token)
    {
        MessageBroker.Default.Publish(new InputTypeData(InputTypeData.InputType.Disable));
        await UniTask.Delay(System.TimeSpan.FromSeconds(_inputDisableTime), cancellationToken: token);
        MessageBroker.Default.Publish(new InputTypeData(InputTypeData.InputType.Player));
    }
}

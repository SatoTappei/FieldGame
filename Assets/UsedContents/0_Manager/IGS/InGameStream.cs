using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

/// <summary>
/// インゲームの流れを制御するクラス
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] InGameStateLineModule _stateLineModule;
    [SerializeField] InGameTimerModule _timerModule;
    [SerializeField] InGameHelicopterMoveModule _heliMoveModule;
    [SerializeField] InGameEscapeAreaModule _escapeAreaModule;

    void Awake()
    {
        _escapeAreaModule.Inactive();

        // タイトル画面からインゲームに遷移したことをトリガーする
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => 
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            Execute(token).Forget();
        }).AddTo(this);
    }

    async UniTaskVoid Execute(CancellationToken token)
    {
        // TODO:敵の台詞の送信に割り込まれるのでプレイヤーの配置に工夫が必要
        _stateLineModule.SendInGameStartLineAsync(token).Forget();
        await _timerModule.Execute(token);
        await _heliMoveModule.Execute(token);
        _escapeAreaModule.Active();
        await _escapeAreaModule.Execute(token, gameObject);
        Debug.Log("脱出");
    }
}
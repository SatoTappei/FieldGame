using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// インゲームの流れを制御するクラス
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] InGameStateLineModule _stateLineModule;
    [SerializeField] InGameTimerModule _timerModule;
    [SerializeField] InGameHelicopterMoveModule _heliMoveModule;
    [SerializeField] InGameEscapeAreaModule _escapeAreaModule;
    [SerializeField] InGameResultModule _resultModule;

    void Awake()
    {
        _escapeAreaModule.Inactive();
        _resultModule.Inactive();

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

        // 脱出してゲーム終了を送信する＆UI操作に切り替える
        MessageBroker.Default.Publish(new InGameClearTrigger());
        MessageBroker.Default.Publish(new InputTypeData(InputTypeData.InputType.UI));

        // リザルト画面でボタンがクリックされるのを待って再読み込み
        _resultModule.ActiveAndSelectResultButton();
        await _resultModule.Execute(token);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

/// <summary>
/// ゲームの進行に応じた台詞を送信するクラス
/// 他のクラスのメッセージ送信と同期していないので割り込まれるので注意が必要
/// </summary>
[System.Serializable]
public class InGameStateLineModule
{
    static readonly string[] InGameStartLines =
    {
        "「アイテムを集めて脱出してね」",
        "「しばらくしたら脱出用のヘリコプターが来るよ」",
        "「頑張れ(∩´∀｀)∩」",
    };

    static readonly float Interval = 2.0f;

    public async UniTaskVoid SendInGameStartLineAsync(CancellationToken token)
    {
        foreach (string line in InGameStartLines)
        {
            MessageBroker.Default.Publish(new LineData("偉い人", line));
            await UniTask.Delay(System.TimeSpan.FromSeconds(Interval), cancellationToken: token);
        }
    }
}

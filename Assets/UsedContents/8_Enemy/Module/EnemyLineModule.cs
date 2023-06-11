using UniRx;
using UnityEngine;

/// <summary>
/// 台詞を表示するためのメッセージを送信するクラス
/// </summary>
[System.Serializable]
public class EnemyLineModule
{
    static readonly string[] DefeatedLines =
    {
        "「あ、兄貴ぃ…もうだめだ…」",
        "「あぼーん」",
        "「ちょっと待ってあのね」",
        "「おびただしい りゅうけつ ！」",
        "「ｱﾂｲｾﾞ ｱﾂｲｾﾞｪｰ ｱﾂｸﾃ ｼﾇｾﾞｪ-ｯ!」"
    };

    static readonly string[] DetectLines =
    {
        "「エモノがいたぜ！」",
        "「ターゲット発見伝！」",
        "「突撃ーっ！」"
    };

    public void SendDetectLineMessage()
    {
        string line = DetectLines[Random.Range(0, DetectLines.Length)];
        MessageBroker.Default.Publish(new LineData("敵", line));
    }

    public void SendDefeatedLineMessage()
    {
        string line = DefeatedLines[Random.Range(0, DefeatedLines.Length)];
        MessageBroker.Default.Publish(new LineData("敵", line));
    }
}

using UniRx;
using UnityEngine;

/// <summary>
/// 台詞を表示するためのメッセージを送信するクラス
/// </summary>
[System.Serializable]
public class EnemyLineModule
{
    static string[] _defeatedLines =
    {
        "「あ、兄貴ぃ…もうだめだ…」",
        "「あぼーん」",
        "「ちょっと待ってあのね」",
        "「おびただしい りゅうけつ ！」",
        "「ｱﾂｲｾﾞ ｱﾂｲｾﾞｪｰ ｱﾂｸﾃ ｼﾇｾﾞｪ-ｯ!」"
    };

    static string[] _detectLines =
    {
        "「エモノがいたぜ！」",
        "「ターゲット発見伝！」",
        "「突撃ーっ！」"
    };

    public void SendDetectLineMessage()
    {
        string line = _detectLines[Random.Range(0, _detectLines.Length)];
        MessageBroker.Default.Publish(new LineData("敵", line));
    }

    public void SendDefeatedLineMessage()
    {
        string line = _defeatedLines[Random.Range(0, _defeatedLines.Length)];
        MessageBroker.Default.Publish(new LineData("敵", line));
    }
}

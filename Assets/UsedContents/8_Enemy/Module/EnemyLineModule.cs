using UniRx;
using UnityEngine;

/// <summary>
/// 台詞を表示するためのメッセージを送信するクラス
/// </summary>
[System.Serializable]
public class EnemyLineModule
{
    static string[] _lines =
    {
        "も、もうだめだ…",
        "あぼーん",
        "ちょっと待ってあのね",
        "ﾔﾗﾚﾁｬｯﾀ",
        "ｱﾂｲｾﾞ ｱﾂｲｾﾞｪｰ ｱﾂｸﾃ ｼﾇｾﾞｪ-ｯ!"
    };

    public void SendDefeatedLineMessage()
    {
        string line = _lines[Random.Range(0, _lines.Length)];
        MessageBroker.Default.Publish(new LineData("敵", line));
    }
}

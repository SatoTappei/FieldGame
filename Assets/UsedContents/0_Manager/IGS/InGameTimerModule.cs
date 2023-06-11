using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// インゲーム中のタイマーを制御するクラス
/// </summary>
[System.Serializable]
public class InGameTimerModule
{
    [SerializeField] Text _text;
    [Header("制限時間")]
    [Tooltip("秒"), Range(0, 60)]
    [SerializeField] int _seconds = 0;
    [Tooltip("分")]
    [SerializeField] int _minute = 3;

    /// <summary>
    /// この値からdeltaTimeを引いた値を分と秒に直して表示する
    /// </summary>
    float _time;
    /// <summary>
    /// 秒が更新された場合にのみテキストを変更するよう前の秒を保持しておく
    /// </summary>
    int _prevSeconds;

    public void InitOnAwake()
    {
        _time = _minute * 60 + _seconds;
        _prevSeconds = _seconds;
    }

    /// <summary>
    /// 呼び出すことでタイマーを更新するので呼び出さなければ一時停止と同じ挙動になる
    /// </summary>
    public void Update()
    {
        if (_time <= 0) return;

        _time -= Time.deltaTime;
        int m = (int)_time / 60;
        int s = (int)(_time - m * 60);

        if (_prevSeconds != s)
        {
            _text.text = ZString.Concat($"{m:00}:{s:00}");
        }

        _prevSeconds = s;
    }
}

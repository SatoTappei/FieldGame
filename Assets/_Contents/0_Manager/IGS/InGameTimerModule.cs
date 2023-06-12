using Cysharp.Text;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
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
    /// 毎フレームdeltaTimeを値から引いていき、その値を分と秒に変換して表示する
    /// </summary>
    public async UniTask Execute(CancellationToken token)
    {
        float time = _minute * 60 + _seconds;
        int prevSeconds = _seconds;

        while (time >= 0)
        {
            token.ThrowIfCancellationRequested();

            time -= Time.deltaTime;
            int m = (int)time / 60;
            int s = (int)(time - m * 60);

            if (prevSeconds != s)
            {
                _text.text = ZString.Concat($"{m:00}:{s:00}");
            }

            prevSeconds = s;

            await UniTask.Yield(token);
        }
    }
}

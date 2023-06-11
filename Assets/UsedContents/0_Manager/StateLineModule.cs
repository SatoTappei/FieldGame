using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// ゲームの進行に応じた台詞を送信するクラス
/// </summary>
//[System.Serializable]
//public class StateLineModule
//{
//    static readonly string[] InGameStartLines =
//    {
//        "「アイテムを集めて脱出してね」",
//        "「しばらくしたら脱出用のヘリコプターが来るよ」",
//        "「頑張れ(∩´∀｀)∩」",
//    };

//    static readonly float Delay = 2.0f;

//    /// <summary>
//    /// 敵の台詞の送信に割り込まれるのでプレイヤーの配置に工夫が必要
//    /// </summary>
//    public async UniTaskVoid SendInGameStartLineAsync(CancellationToken token)
//    {
//        foreach(string line in InGameStartLines)
//        {
//            MessageBroker.Default.Publish(new LineData("偉い人", line));
//            await UniTask.Delay(System.TimeSpan.FromSeconds(Delay), cancellationToken: token);
//        }
//    }
//}

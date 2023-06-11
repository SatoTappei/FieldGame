using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// インゲームの流れを制御するクラス
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] InGameStateLineModule _stateLineModule;
    [SerializeField] InGameTimerModule _timerModule;

    void Awake()
    {
        _timerModule.InitOnAwake();

        bool inGameStart = false;
        // タイトル画面からインゲームに遷移したことをトリガーする
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => 
        {
            // インゲーム開始のフラグを立てる
            inGameStart = true;
            // 敵の台詞の送信に割り込まれるのでプレイヤーの配置に工夫が必要
            _stateLineModule.SendInGameStartLineAsync(this.GetCancellationTokenOnDestroy()).Forget();

            // インゲーム開始のフラグが経ってから毎フレーム更新される
            this.UpdateAsObservable()
                .TakeWhile(_ => inGameStart)
                .Subscribe(_ =>
                {
                    _timerModule.Update();
                });
        }).AddTo(this);
       
    }
}

// ゲーム開始時にメッセージ
// ある程度時間が経過したらもうちょっとでつくメッセージ
// 到着したら到着したメッセージ
// 脱出したらクリア、シーンをリロード

// 必要: インゲームのタイマー
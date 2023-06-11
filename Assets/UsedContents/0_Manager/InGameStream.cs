using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// インゲームの流れを制御するクラス
/// </summary>
//public class InGameStream : MonoBehaviour
//{
//    //[SerializeField] StateLineModule _stateLineModule;

//    void Awake()
//    {
//        // タイトル画面からインゲームに遷移したことをトリガーする
//        //MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_=>
//        //{
//        //    _stateLineModule.SendInGameStartLineAsync(this.GetCancellationTokenOnDestroy()).Forget();
//        //}).AddTo(this);
//    }
//}

// ゲーム開始時にメッセージ
// ある程度時間が経過したらもうちょっとでつくメッセージ
// 到着したら到着したメッセージ
// 脱出したらクリア、シーンをリロード

// 必要: インゲームのタイマー
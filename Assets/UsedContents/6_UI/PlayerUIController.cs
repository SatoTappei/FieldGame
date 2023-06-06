using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// インゲーム中のプレイヤーUIを制御する
/// </summary>
public class PlayerUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup _playerUIRoot;
    [SerializeField] LifePointGaugeModule _lifePointGaugeModule;
    [SerializeField] LineTextModule _lineTextModule;

    void Awake()
    {
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => Active()).AddTo(gameObject);
        _lifePointGaugeModule.InitOnAwake(gameObject);
        _lineTextModule.InitOnAwake(gameObject);

        // 台詞を表示するUIのタイマーを毎フレーム更新する
        this.UpdateAsObservable().Subscribe(_=> _lineTextModule.Update());
    }

    void Start()
    {
        // タイトル画面では透明にしている
        _playerUIRoot.alpha = 0;
    }

    void Active() => _playerUIRoot.alpha = 1;
}

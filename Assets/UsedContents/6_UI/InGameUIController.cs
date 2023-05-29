using UnityEngine;
using UniRx;

/// <summary>
/// ƒCƒ“ƒQ[ƒ€’†‚ÌUI‚ğ§Œä‚·‚é
/// </summary>
public class InGameUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup _playerUIRoot;
    [SerializeField] LifePointGaugeModule _lifePointGaugeModule;

    void Awake()
    {
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => Active());
        _lifePointGaugeModule.InitOnAwake(gameObject);
    }

    void Start()
    {
        _playerUIRoot.alpha = 0;
    }

    void Active() => _playerUIRoot.alpha = 1;
}

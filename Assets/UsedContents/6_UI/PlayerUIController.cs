using UnityEngine;
using UniRx;

/// <summary>
/// �C���Q�[�����̃v���C���[UI�𐧌䂷��
/// </summary>
public class PlayerUIController : MonoBehaviour
{
    [SerializeField] CanvasGroup _playerUIRoot;
    [SerializeField] LifePointGaugeModule _lifePointGaugeModule;

    void Awake()
    {
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => Active()).AddTo(gameObject);
        _lifePointGaugeModule.InitOnAwake(gameObject);
    }

    void Start()
    {
        _playerUIRoot.alpha = 0;
    }

    void Active() => _playerUIRoot.alpha = 1;
}

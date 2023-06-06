using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �C���Q�[�����̃v���C���[UI�𐧌䂷��
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

        // �䎌��\������UI�̃^�C�}�[�𖈃t���[���X�V����
        this.UpdateAsObservable().Subscribe(_=> _lineTextModule.Update());
    }

    void Start()
    {
        // �^�C�g����ʂł͓����ɂ��Ă���
        _playerUIRoot.alpha = 0;
    }

    void Active() => _playerUIRoot.alpha = 1;
}

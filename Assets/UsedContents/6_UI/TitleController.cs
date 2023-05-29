using DG.Tweening;
using UniRx;
using UnityEngine;

/// <summary>
/// �^�C�g����ʂ̐�����s���N���X
/// </summary>
public class TitleController : MonoBehaviour
{
    [SerializeField] CanvasGroup _titleUIRoot;
    [SerializeField] TitleButtonModule _gameStartButtonModule;
    [SerializeField] TitleButtonModule _quitButtonModule;
    [Header("�{�^���������Ă��珈��������܂ł̃f�B���C")]
    [SerializeField] float _transitionDelay = .5f;

    void Start()
    {
        _gameStartButtonModule.InitOnStart();
        _quitButtonModule.InitOnStart();

        _gameStartButtonModule.OnSubmit += InGameStart;
        _quitButtonModule.OnSubmit += ApplicationQuit;
    }

    void InGameStart()
    {
        // �^�C�g����UI�𑀍�ł��Ȃ��悤�ɂ���
        _titleUIRoot.interactable = false;
        _titleUIRoot.blocksRaycasts = false;

        // UI�S�̂̉��o��A�C���Q�[���J�n�̃��b�Z�[�W���O������
        _titleUIRoot.DOFade(0, _transitionDelay).OnComplete(() => 
        {
            MessageBroker.Default.Publish(new ToInGameTrigger());
            MessageBroker.Default.Publish(new InputTypeData(InputTypeData.InputType.Player));
        });
    }

    void ApplicationQuit()
    {
        // TODO: �Q�[���I���̏���������
    }
}

using DG.Tweening;
using UniRx;
using UnityEngine;

/// <summary>
/// タイトル画面の制御を行うクラス
/// </summary>
public class TitleController : MonoBehaviour
{
    [SerializeField] CanvasGroup _titleUIRoot;
    [SerializeField] TitleButtonModule _gameStartButtonModule;
    [SerializeField] TitleButtonModule _quitButtonModule;
    [Header("ボタンを押してから処理が走るまでのディレイ")]
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
        // タイトルのUIを操作できないようにする
        _titleUIRoot.interactable = false;
        _titleUIRoot.blocksRaycasts = false;

        // UI全体の演出後、インゲーム開始のメッセージングをする
        _titleUIRoot.DOFade(0, _transitionDelay).OnComplete(() => 
        {
            MessageBroker.Default.Publish(new ToInGameTrigger());
            MessageBroker.Default.Publish(new InputTypeData(InputTypeData.InputType.Player));
        });
    }

    void ApplicationQuit()
    {
        // TODO: ゲーム終了の処理を書く
    }
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �e��{�^����Navigation��ݒ肷��N���X
/// </summary>
public class ButtonNavigationSettings : MonoBehaviour
{
    [Header("�^�C�g���̃{�^��")]
    [SerializeField] Selectable _startButton;
    [SerializeField] Selectable _quitButton;
    [Header("���U���g�̃{�^��")]
    [SerializeField] Selectable _resultButton;

    void Start()
    {
        Navigation startButtonNav = _startButton.navigation;
        startButtonNav.mode = Navigation.Mode.Explicit;
        startButtonNav.selectOnUp = null;
        startButtonNav.selectOnLeft = null;
        startButtonNav.selectOnRight = null;
        startButtonNav.selectOnDown = _quitButton;
        _startButton.navigation = startButtonNav;

        Navigation quitButtonNav = _quitButton.navigation;
        quitButtonNav.mode = Navigation.Mode.Explicit;
        quitButtonNav.selectOnUp = _startButton;
        quitButtonNav.selectOnLeft = null;
        quitButtonNav.selectOnRight = null;
        quitButtonNav.selectOnDown = null;
        _quitButton.navigation = quitButtonNav;

        Navigation resultButtonNav = _resultButton.navigation;
        resultButtonNav.mode = Navigation.Mode.Explicit;
        resultButtonNav.selectOnUp = null;
        resultButtonNav.selectOnLeft = null;
        resultButtonNav.selectOnRight = null;
        resultButtonNav.selectOnDown = null;
        _resultButton.navigation = resultButtonNav;
    }
}

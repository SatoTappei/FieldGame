using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �^�C�g������C���Q�[���Ɉڂ�ۂɃJ������؂�ւ���
/// </summary>
public class CameraSwitchModule : MonoBehaviour
{
    static readonly int UnSelectedPriority = 1;
    static readonly int SelectedPriority = 11;

    [Header("�؂�ւ���J����")]
    [SerializeField] CinemachineVirtualCamera _titleCamera;
    [SerializeField] CinemachineVirtualCamera _inGameCamera;
    [Header("�؂�ւ��̏��������蓖�Ă�{�^��")]
    [SerializeField] Button _gameStartButton;

    void Awake()
    {
        _titleCamera.Priority = SelectedPriority;
        _inGameCamera.Priority = UnSelectedPriority;

        _gameStartButton.onClick.AddListener(ChangeCamera);
    }

    /// <summary>
    /// �^�C�g���̃J��������C���Q�[���̃J�����Ɋ��炩�ɑJ�ڂ���
    /// </summary>
    void ChangeCamera()
    {
        _titleCamera.Priority = UnSelectedPriority;
        _inGameCamera.Priority = SelectedPriority;
    }
}

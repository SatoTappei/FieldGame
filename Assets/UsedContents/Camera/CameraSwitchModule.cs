using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// タイトルからインゲームに移る際にカメラを切り替える
/// </summary>
public class CameraSwitchModule : MonoBehaviour
{
    static readonly int UnSelectedPriority = 1;
    static readonly int SelectedPriority = 11;

    [Header("切り替えるカメラ")]
    [SerializeField] CinemachineVirtualCamera _titleCamera;
    [SerializeField] CinemachineVirtualCamera _inGameCamera;
    [Header("切り替えの処理を割り当てるボタン")]
    [SerializeField] Button _gameStartButton;

    void Awake()
    {
        _titleCamera.Priority = SelectedPriority;
        _inGameCamera.Priority = UnSelectedPriority;

        _gameStartButton.onClick.AddListener(ChangeCamera);
    }

    /// <summary>
    /// タイトルのカメラからインゲームのカメラに滑らかに遷移する
    /// </summary>
    void ChangeCamera()
    {
        _titleCamera.Priority = UnSelectedPriority;
        _inGameCamera.Priority = SelectedPriority;
    }
}

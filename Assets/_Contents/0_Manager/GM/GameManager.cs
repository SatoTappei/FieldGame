using UnityEngine;

/// <summary>
/// ���󉹂�炷�@�\�̂ݎ����Ă���N���X
/// Singleton����DDOL�ł͂Ȃ��̂ŃV�[���J�ڂŏ�����
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] AudioModule _audioModule;

    public AudioModule AudioModule => _audioModule;

    void Awake()
    {
        Application.targetFrameRate = 60;

        if (Instance == null)
        {
            Instance = this;
            _audioModule.InitOnAwake(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}

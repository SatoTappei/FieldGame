using UnityEngine;

/// <summary>
/// Singleton����DDOL�ɓo�^���Ă��Ȃ��̂ŃV�[���J�ڂŏ�����
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("�炵�������̃f�[�^")]
    [SerializeField] AudioData[] _audioDatas;
    AudioModule _audioModule;

    public AudioModule AudioModule => _audioModule;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _audioModule = new AudioModule(gameObject, _audioDatas);
        }
        else
        {
            Destroy(this);
        }
    }
}

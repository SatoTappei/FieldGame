using UnityEngine;

/// <summary>
/// Singleton����DDOL�ɓo�^���Ă��Ȃ��̂ŃV�[���J�ڂŏ�����
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

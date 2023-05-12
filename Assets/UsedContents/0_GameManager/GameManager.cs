using UnityEngine;

/// <summary>
/// SingletonだがDDOLに登録していないのでシーン遷移で消える
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("鳴らしたい音のデータ")]
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

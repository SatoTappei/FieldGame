using UnityEngine;

/// <summary>
/// SingletonだがDDOLに登録していないのでシーン遷移で消える
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] AudioModule _audioModule;

    public AudioModule AudioModule => _audioModule;

    void Awake()
    {
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

using UnityEngine;

/// <summary>
/// 現状音を鳴らす機能のみ持っているクラス
/// SingletonだがDDOLではないのでシーン遷移で消える
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

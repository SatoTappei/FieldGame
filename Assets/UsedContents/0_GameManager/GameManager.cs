using UnityEngine;

/// <summary>
/// Singleton‚¾‚ªDDOL‚É“o˜^‚µ‚Ä‚¢‚È‚¢‚Ì‚ÅƒV[ƒ“‘JˆÚ‚ÅÁ‚¦‚é
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

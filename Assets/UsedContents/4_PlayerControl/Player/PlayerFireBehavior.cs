using UnityEngine;

/// <summary>
/// プレイヤーの攻撃に関する処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerFireBehavior : IInputActionRegistrable
{
    [Header("攻撃のレート")]
    [SerializeField] float _attackRate = 0.33f;
    [Header("攻撃時に再生するParticle")]
    [SerializeField] ParticleSystem _fireParticle;

    float _time;
    /// <summary>
    /// InputSystemに登録して入力のオンオフで攻撃中フラグを切り替える
    /// </summary>
    bool _isFiring;

    public void RegisterInputAction(InputActionRegister register)
    {
        register.OnFire += () => OpenFire();
        register.OnFireCanceled += () => _isFiring = false;
    }

    /// <summary>
    /// 最初の1発は射撃ボタンを押したタイミングで必ず発射される
    /// 射撃ボタンを連打することで攻撃レート以上の早さで発射可能
    /// </summary>
    void OpenFire()
    {
        _isFiring = true;
        _time = _attackRate;
    }

    public void Update()
    {
        _time += Time.deltaTime;

        if (_isFiring && _time > _attackRate)
        {
            _time = 0;
            GameManager.Instance.AudioModule.PlaySE(AudioType.SE_Fire);
            _fireParticle.Play();
        }
    }
}

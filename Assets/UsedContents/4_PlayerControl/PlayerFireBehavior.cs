using UnityEngine;

/// <summary>
/// �v���C���[�̍U���Ɋւ��鏈�����s���N���X
/// </summary>
[System.Serializable]
public class PlayerFireBehavior : IInputActionRegistrable
{
    [Header("�U���̃��[�g")]
    [SerializeField] float _attackRate = 0.33f;
    [Header("�U�����ɍĐ�����Particle")]
    [SerializeField] ParticleSystem _fireParticle;

    float _time;
    /// <summary>
    /// InputSystem�ɓo�^���ē��͂̃I���I�t�ōU�����t���O��؂�ւ���
    /// </summary>
    bool _isFiring;

    public void RegisterInputAction(InputActionRegister register)
    {
        register.OnFire += () => OpenFire();
        register.OnFireCanceled += () => _isFiring = false;
    }

    /// <summary>
    /// �ŏ���1���͎ˌ��{�^�����������^�C�~���O�ŕK�����˂����
    /// �ˌ��{�^����A�ł��邱�ƂōU�����[�g�ȏ�̑����Ŕ��ˉ\
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

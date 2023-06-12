using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Threading;
using Cysharp.Threading.Tasks;

/// <summary>
/// �e�U�镑���̃N���X��p���ăv���C���[�𐧌䂷��N���X
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerFireBehavior _playerFireBehavior;
    [SerializeField] PlayerMoveBehavior _playerMoveBehavior;
    [SerializeField] PlayerDefeatedBehavior _playerDefeatedBehavior;
    [SerializeField] InvisibleBehavior _invisibleBehavior;
    [SerializeField] CameraControlModule _cameraControlModule;
    [SerializeField] PlayerAnimModule _animModule;
    [SerializeField] PlayerLifePointModule _lifePointModule;
    [SerializeField] PlayerAimRaycastModule _aimRaycastModule;

    PlayerInputRegister _inputActionRegister;
    Transform _transform;

    void Awake()
    {
        InitOnAwake();
    }

    void InitOnAwake()
    {
        // �E�o���ăQ�[���N���A�����^�C�~���O�Ŗ��G��ԂɂȂ�
        MessageBroker.Default.Receive<InGameClearTrigger>().Subscribe(_ => 
        {
            _invisibleBehavior.Active(gameObject);
        }).AddTo(gameObject);

        // InputSystem�ւ̑���̓o�^
        _inputActionRegister = new PlayerInputRegister(gameObject);
        _playerMoveBehavior.RegisterInputAction(_inputActionRegister);
        _playerFireBehavior.RegisterInputAction(_inputActionRegister);
        _cameraControlModule.RegisterInputAction(_inputActionRegister);
        _animModule.RegisterInputAction(_inputActionRegister);
        
        // �e�N���X�̏�����
        _playerFireBehavior.InitOnAwake();
        _lifePointModule.InitOnAwake(transform);

        // �_���[�W���󂯂đ̗͂����������ۂɃ_���[�W�̃A�j���[�V�������Đ�����
        _lifePointModule.CurrentLifePoint.Skip(1).Subscribe(_ => 
        {
            _animModule.PlayDamageAnim();
            GameManager.Instance.AudioModule.PlaySE(AudioType.SE_PlayerDamage);
        }).AddTo(gameObject);

        // �_���[�W���󂯂đ̗͂�0�ɂȂ����ۂɎ��S�̉��o���Đ�����
        _lifePointModule.CurrentLifePoint.Where(lifePoint => lifePoint == 0)
        .Subscribe(_ => 
        {
            // �̗͂�S�񕜂��Ĉʒu�������ʒu�ɖ߂�
            _playerDefeatedBehavior.Respawn(_transform);
            _lifePointModule.Reset();
            
            // ���x��0�Ƀ��Z�b�g���Ĉ�莞�ԓ��͂��󂯕t���Ȃ�
            // ���̏������Ă΂�Ă�Update()�ōX�V����Ă��郁�\�b�h�͉e�����󂯂Ȃ���
            // ���͂��󂯕t���Ȃ��Ԃ�Update()�̏������Ă΂�Ă���̂Œ���
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            _playerDefeatedBehavior.PlayerInputDisableDelayedEnableAsync(token).Forget();
            _playerMoveBehavior.ResetIdleVelocity();

        }).AddTo(gameObject);

        // 1�t���[�����̏��������Ă���̂ŃI�u�W�F�N�g���\���ɂ���ΐ���Ɏ~�܂�
        this.UpdateAsObservable().Subscribe(_ => 
        {
            CameraMode mode = _cameraControlModule.CurrentCameraMode;
            _playerMoveBehavior.Update(mode);
            _playerFireBehavior.Update();
            _cameraControlModule.Update();
            _animModule.Update();
            _aimRaycastModule.Update(mode);
        });
        this.FixedUpdateAsObservable().Subscribe(_ =>
        {
            _playerMoveBehavior.FixedUpdate();
        });

        // �Q�[���I�����ɃI�u�W�F�N�g���j�������^�C�~���O�Ō㏈��
        this.OnDestroyAsObservable().Subscribe(_ =>
        {
            _playerFireBehavior.Dispose();
        });

        _transform = transform;
    }

    void OnDrawGizmos()
    {
        _aimRaycastModule.Visualize();
    }
}

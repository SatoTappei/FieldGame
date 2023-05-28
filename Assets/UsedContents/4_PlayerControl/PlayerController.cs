using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// �e�U�镑���̃N���X��p���ăv���C���[�𐧌䂷��N���X
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerFireBehavior _playerFireBehavior;
    [SerializeField] PlayerMoveBehavior _playerMoveBehavior;
    [SerializeField] CameraControlModule _cameraControlModule;
    PlayerInputRegister _inputActionRegister;
    Transform _transform;

    void Awake()
    {
        InitOnAwake();
    }

    void InitOnAwake()
    {
        _inputActionRegister = new PlayerInputRegister();
        _playerMoveBehavior.RegisterInputAction(_inputActionRegister);
        _playerFireBehavior.RegisterInputAction(_inputActionRegister);
        _cameraControlModule.RegisterInputAction(_inputActionRegister);

        this.UpdateAsObservable().Subscribe(_ => 
        {
            CameraMode mode = _cameraControlModule.CurrentCameraMode;
            _playerMoveBehavior.Update(_transform, mode);
            _playerFireBehavior.Update();
            _cameraControlModule.Update();
        });

        _transform = transform;
    }
}

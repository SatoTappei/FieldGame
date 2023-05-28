using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

/// <summary>
/// 現在のカメラモードの列挙型
/// CameraControlModuleで変更を行う
/// </summary>
public enum CameraMode
{
    Freelook,
    ADS,
}

/// <summary>
/// プレイヤーのカメラの操作を行うクラス
/// </summary>
[System.Serializable]
public class CameraControlModule : IInputActionRegistrable
{
    [Header("Y軸方向の回転を反映するオブジェクト")]
    [SerializeField] Transform _horiFollowTarget;
    [Header("X軸方向の回転を反映するオブジェクト")]
    [SerializeField] Transform _vertFollowTarget;
    [Header("感度")]
    [SerializeField] int _sensitivity = 160;
    [Header("X軸方向の角度の範囲")]
    [SerializeField] float _angleMax = 45.0f;
    [SerializeField] float _angleMin = -45.0f;
    [Header("0:Freelook 1:ADS")]
    [SerializeField] CinemachineVirtualCamera[] _cameras;
    [Header("Freelookにした際に回転を読み取るオブジェクト")]
    [SerializeField] Transform _model;

    /// <summary>
    /// 入力をカメラの移動方向(正規化済み)に変換した値
    /// </summary>
    Vector3 _dir;
    /// <summary>
    /// 現在のカメラモード
    /// 次のフレームで移動を更新する際に参照される
    /// </summary>
    public CameraMode CurrentCameraMode { get; private set; }

    public void RegisterInputAction(PlayerInputRegister register)
    {
        register.OnCameraMove += UpdateNormalizedDirection;
        register.OnCameraMoveCanceled += UpdateNormalizedDirection;
        register.OnFocus += SwitchCamera;
        register.OnFocusCanceled += SwitchCamera;
    }

    public void Update()
    {
        // カメラ操作(水平)
        Vector3 cameraMovement = _dir * Time.deltaTime * _sensitivity;
        _horiFollowTarget.Rotate(new Vector3(0, cameraMovement.y, 0));

        // カメラ操作(垂直)
        float rotX = _vertFollowTarget.localEulerAngles.x;
        rotX -= cameraMovement.x;
        if (rotX > 180) rotX -= 360;
        rotX = Mathf.Clamp(rotX, _angleMin, _angleMax);
        _vertFollowTarget.localEulerAngles = new Vector3(rotX, 0, 0);
    }

    void SwitchCamera()
    {
        int index = (int)CurrentCameraMode;

        // 使用するカメラの変更
        _cameras[index].Priority = 10;
        index = 1 - index;
        _cameras[index].Priority = 11;

        // ADSからFreelookに戻るとき、カメラがズレないようにrotationをADSに合わせる
        if (index == 0)
        {
            _vertFollowTarget.rotation = Quaternion.identity;
            _horiFollowTarget.rotation = _model.rotation;
        }

        CurrentCameraMode = (CameraMode)index;
    }

    /// <summary>
    /// カメラの方向の更新
    /// 一定間隔で入力を受け取るようInputSystemに登録される
    /// </summary>
    void UpdateNormalizedDirection(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.y, value.x, 0).normalized;
    }
}

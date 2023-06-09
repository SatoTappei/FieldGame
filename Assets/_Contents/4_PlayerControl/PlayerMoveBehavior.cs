using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動に関する処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerMoveBehavior : IInputActionRegistrable
{
    [SerializeField] Rigidbody _rigidbody;
    [Header("移動速度")]
    [SerializeField] float _speed = 5.0f;
    [Header("走る際の移動速度の倍率")]
    [SerializeField] float _runMag = 1.5f;
    [Header("移動に応じた方向に振り向かせるオブジェクト")]
    [SerializeField] Transform _model;
    [Header("振り向き速度")]
    [SerializeField] float _rotSpeed = 20.0f;

    Vector3 _velo;
    /// <summary>
    /// 入力を移動方向(正規化済み)に変換した値
    /// </summary>
    Vector3 _dir;
    /// <summary>
    /// InputSystemに登録して入力のオンオフで走り中フラグを切り替える
    /// </summary>
    bool _isRunning;

    public void RegisterInputAction(PlayerInputRegister register)
    {
        register.OnMove += UpdateNormalizedDirection;
        register.OnMoveCanceled += UpdateNormalizedDirection;
        register.OnRun += () => _isRunning = true;
        register.OnRunCanceled += () => _isRunning = false;
    }

    public void Update(CameraMode mode)
    {
        // Y軸を回転軸とした回転
        Quaternion cameraRot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        // 速度の更新
        Vector3 velo = cameraRot * _dir * _speed * (_isRunning ? _runMag : 1);
        velo.y = _rigidbody.velocity.y;
        _velo = velo;

        // Freelook時には移動方向に向く
        if (mode == CameraMode.Freelook)
        {
            if (velo.x == 0 && velo.z == 0) return;

            Quaternion rot = Quaternion.LookRotation(cameraRot * _dir, Vector3.up);
            _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
        }
        // ADS時にはカメラの中央を向く
        else if(mode == CameraMode.ADS)
        {
            if (Mathf.Approximately(_model.rotation.y, Camera.main.transform.rotation.y)) return;

            Quaternion rot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
            rot.x = 0;
            rot.z = 0;
            _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
        }
    }

    public void FixedUpdate()
    {
        _rigidbody.velocity = _velo;
    }

    /// <summary>
    /// 移動方向の更新
    /// 一定間隔で入力を受け取るようInputSystemに登録される
    /// </summary>
    void UpdateNormalizedDirection(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.x, 0, value.y).normalized;
    }

    /// <summary>
    /// 移動速度を強制的に0にする事で、プレイヤーがリスポーンした際に滑っていくのを防ぐ
    /// </summary>
    public void ResetIdleVelocity()
    {
        _isRunning = false;
        _dir = Vector3.zero;
    }
}

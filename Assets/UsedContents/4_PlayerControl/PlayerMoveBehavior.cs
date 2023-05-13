using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動に関する処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerMoveBehavior : IInputActionRegistrable
{
    [Header("移動速度")]
    [SerializeField] float _speed = 5.0f;
    [Header("走る際の移動速度の倍率")]
    [SerializeField] float _runMag = 1.5f;
    [Header("移動に応じた方向に向かせるTransform")]
    [SerializeField] Transform _model;
    [Header("向く速度")]
    [SerializeField] float _rotSpeed = 20;

    /// <summary>
    /// 入力を進む方向(正規化済み)に変換した値
    /// </summary>
    Vector3 _dir;
    /// <summary>
    /// InputSystemに登録して入力のオンオフで走り中フラグを切り替える
    /// </summary>
    bool _isRunning;

    public void RegisterInputAction(InputActionRegister register)
    {
        register.OnMove += UpdateVelocity;
        register.OnMoveCanceled += UpdateVelocity;
        register.OnRun += () => _isRunning = true;
        register.OnRunCanceled += () => _isRunning = false;
    }

    public void Update(Transform transform)
    {
        // 移動
        Vector3 movement = _dir * Time.deltaTime * _speed * (_isRunning ? _runMag : 1);
        transform.Translate(movement);

        // 移動方向に向かせる
        if (movement != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(_dir, Vector3.up);
            _model.rotation = Quaternion.Lerp(_model.rotation, rot, Time.deltaTime * _rotSpeed);
        }
    }

    /// <summary>
    /// 移動速度の更新
    /// 一定間隔で入力を受け取るようInputSystemに登録される
    /// </summary>
    void UpdateVelocity(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.x, 0, value.y).normalized;
    }
}

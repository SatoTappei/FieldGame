using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの移動に関する処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerMoveBehavior
{
    [Header("移動速度")]
    [SerializeField] float _speed = 5.0f;
    [Header("走る際の移動速度の倍率")]
    [SerializeField] float _runMag = 1.5f;

    /// <summary>
    /// 入力に移動速度を乗算して正規化した値
    /// </summary>
    Vector3 _velo;
    /// <summary>
    /// InputSystemに登録して走るフラグを切り替えるためにSetGetどちらも公開する必要がある
    /// </summary>
    public bool IsRunning { get; set; }

    public void Update(Transform transform)
    {
        float mag = IsRunning ? _runMag : 1;
        transform.Translate(_velo * Time.deltaTime * mag);
    }

    /// <summary>
    /// 移動速度の更新
    /// 一定間隔で入力を受け取るようInputSystemに登録される
    /// </summary>
    public void UpdateVelocity(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _velo = new Vector3(value.x, 0, value.y).normalized * _speed;
    }
}

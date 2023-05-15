using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーに張り付けるだけで使用可能
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SimplePlayerController : MonoBehaviour
{
    PlayerInputActions _inputActions;
    Vector3 _dir;

    void Awake()
    {
        _inputActions = new();
        _inputActions.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMove;

        #region 未使用: 入力に対して8方向の辞書を対応させるコード
        Dictionary<(int v, int h), Vector3> _dirDict;
        _dirDict = new()
        {
            { (0, 0), Vector3.zero },
            { (1, 0), Vector3.forward },
            { (-1, 0), Vector3.back },
            { (0, -1), Vector3.left },
            { (0, 1), Vector3.right },
            
            { (1, 1), (Vector3.forward + Vector3.right).normalized },
            { (-1, 1), (Vector3.back + Vector3.right).normalized },
            { (1, -1), (Vector3.forward + Vector3.left).normalized },
            { (-1, -1), (Vector3.back + Vector3.left).normalized },
        };
        //int v = (int)Input.GetAxisRaw("Vertical");
        //int h = (int)Input.GetAxisRaw("Horizontal");
        //_dir = _dirDict[(v, h)];
        #endregion
    }

    void Update()
    {
        transform.Translate(_dir * Time.deltaTime * 5.0f);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        Vector2 value = context.ReadValue<Vector2>();
        _dir = new Vector3(value.x, 0, value.y).normalized;
    }
}

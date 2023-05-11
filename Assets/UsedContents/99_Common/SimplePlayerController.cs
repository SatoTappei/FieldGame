using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーに張り付けるだけで使えるシンプルなコントローラー
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class SimplePlayerController : MonoBehaviour
{
    Rigidbody _rb;
    Dictionary<(int v, int h), Vector3> _dirDict;
    Vector3 _dir;
    int _speed;

    void Awake()
    {
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

        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        int v = (int)Input.GetAxisRaw("Vertical");
        int h = (int)Input.GetAxisRaw("Horizontal");

        _dir = _dirDict[(v, h)];
        _speed = Input.GetKey(KeyCode.LeftShift) ? 10 : 5;
    }

    void FixedUpdate()
    { 
        _rb.velocity = _dir * _speed;
    }
}

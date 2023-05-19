using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面上にFPSを表示するスクリプト
/// </summary>
public class FPSView : MonoBehaviour
{
    [SerializeField] float _interval = 0.5f;

    GUIStyle _style = new();
    float _timeCount;
    float _timer;
    float _fps;
    int _frameCount;

    void Start()
    {
        _style.fontSize = 30;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        _timeCount += Time.timeScale / Time.deltaTime;
        _frameCount++;

        if (_timer > _interval)
        {
            _timer = 0;
            _fps = _timeCount / _frameCount;
            _timeCount = 0;
            _frameCount = 0;
        }
    }

    void OnGUI()
    {
        GUILayout.Label($"FPS: {_fps:f2}", _style);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviorTreeで使用する各種データを書き込むクラス
/// </summary>
public class BehaviorTreeBlackBoard
{
    float _time = 0;

    public bool IsTimeElapsed()
    {
        _time += Time.deltaTime;
        return _time > 2.0f;
    }
}

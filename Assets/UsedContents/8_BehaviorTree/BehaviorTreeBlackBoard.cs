using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BehaviorTree�Ŏg�p����e��f�[�^���������ރN���X
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

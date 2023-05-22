using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アニメーションの再生を行うクラス
/// </summary>
[System.Serializable]
public class EnemyAnimationModule
{
    /// <summary>
    /// アニメーションの再生をする際に使用する列挙型
    /// ハッシュに変換するのでアニメーション名と同じでなければならない
    /// </summary>
    public enum AnimType
    {
        Idle,
        Move,
        Defeated,
    }

    [SerializeField] Animator _animator;

    Dictionary<AnimType, int> _dict = new();

    public void InitOnAwake()
    {
        foreach (AnimType type in Enum.GetValues(typeof(AnimType)))
        {
            _dict.Add(type, Animator.StringToHash(type.ToString()));
        }
    }

    public void Play(AnimType type)
    {
        _animator.Play(_dict[type]);
    }
}

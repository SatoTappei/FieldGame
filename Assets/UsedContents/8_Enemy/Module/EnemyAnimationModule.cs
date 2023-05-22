using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �A�j���[�V�����̍Đ����s���N���X
/// </summary>
[System.Serializable]
public class EnemyAnimationModule
{
    /// <summary>
    /// �A�j���[�V�����̍Đ�������ۂɎg�p����񋓌^
    /// �n�b�V���ɕϊ�����̂ŃA�j���[�V�������Ɠ����łȂ���΂Ȃ�Ȃ�
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// BehaviorTree�Ŏg�p����m�[�h�̃N���X
/// </summary>
public abstract class BehaviorTreeNode
{
    public enum State
    {
        Runnning,
        Failure,
        Success,
    }

    /// <summary>
    /// �Փ˂����m����^�O
    /// </summary>
    static string HitTag = "Player";

    public BehaviorTreeNode() { }
    public BehaviorTreeNode(Collider collider)
    {
        // �C�ӂ̃^�O�����I�u�W�F�N�g�ƂԂ�������t���O������
        collider.OnTriggerEnterAsObservable()
            .Where(c => c.CompareTag(HitTag))
            .Subscribe(_ => FlagControlAsync().Forget());
    }

    State _currentState;
    bool _isActive;
    /// <summary>
    /// �Փ˂����ۂ�1�t���[���������t���O
    /// </summary>
    protected bool IsTriggerEnter { get; private set; }

    /// <summary>
    /// Tree�̃N���X����Update()�̃^�C�~���O�ŌĂ΂�郁�\�b�h
    /// �ŏ���1���"OnEnter()+OnStay()"���Ă΂��B
    /// OnStay()��Running�ȊO��Ԃ����ꍇ��"OnStay()+OnExit()"
    /// </summary>
    public State Update()
    {
        if (!_isActive)
        {
            _isActive = true;
            OnEnter();
        }

        _currentState = OnStay();

        if (_currentState == State.Failure || _currentState == State.Success)
        {
            OnExit();
            _isActive = false;
        }

        return _currentState;
    }

    protected abstract void OnEnter();
    protected abstract State OnStay();
    protected abstract void OnExit();

    async UniTaskVoid FlagControlAsync()
    {
        IsTriggerEnter = true;
        await UniTask.Yield();
        IsTriggerEnter = false;
    }
}
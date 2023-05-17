using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

/// <summary>
/// BehaviorTreeで使用するノードのクラス
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
    /// 衝突を検知するタグ
    /// </summary>
    static string HitTag = "Player";

    public BehaviorTreeNode() { }
    public BehaviorTreeNode(Collider collider)
    {
        // 任意のタグを持つオブジェクトとぶつかったらフラグが立つ
        collider.OnTriggerEnterAsObservable()
            .Where(c => c.CompareTag(HitTag))
            .Subscribe(_ => FlagControlAsync().Forget());
    }

    State _currentState;
    bool _isActive;
    /// <summary>
    /// 衝突した際に1フレームだけ立つフラグ
    /// </summary>
    protected bool IsTriggerEnter { get; private set; }

    /// <summary>
    /// TreeのクラスからUpdate()のタイミングで呼ばれるメソッド
    /// 最初の1回は"OnEnter()+OnStay()"が呼ばれる。
    /// OnStay()がRunning以外を返した場合は"OnStay()+OnExit()"
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
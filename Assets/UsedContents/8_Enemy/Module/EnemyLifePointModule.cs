using UniRx;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �̗͂̐�����s���N���X
/// </summary>
[System.Serializable]
public class EnemyLifePointModule
{
    public event UnityAction OnDamaged;
    public event UnityAction OnDefeated;

    /// <summary>
    /// �e���q�b�g�����獕�̗̑͂̒l������������
    /// </summary>
    public void InitOnAwake(Transform transform, BehaviorTreeBlackBoard blackBoard)
    {
        MessageBroker.Default.Receive<DamageData>()
            .Where(data => transform.CompareTag(data.Tag))
            .Where(data => (data.HitPos - transform.position).sqrMagnitude < data.Range * data.Range)
            .Where(_ => blackBoard.LifePoint > 0)
            .Subscribe(_ => 
            {
                OnDamaged?.Invoke();
                if (--blackBoard.LifePoint <= 0)
                {
                    OnDefeated?.Invoke();
                }
            }).AddTo(transform);
    }
}
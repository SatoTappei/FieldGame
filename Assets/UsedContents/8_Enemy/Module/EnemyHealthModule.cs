using UniRx;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �̗͂̐�����s���N���X
/// </summary>
[System.Serializable]
public class EnemyHealthModule
{
    public UnityAction OnDamaged;

    public void InitOnAwake(Transform transform)
    {
        // �e�����������ʒu�Ƃ̋�����Range�ȉ��������ꍇ�͒e���q�b�g����
        MessageBroker.Default.Receive<DamageData>()
            .Where(data => transform.CompareTag(data.TargetTag))
            .Where(data => (data.HitPos - transform.position).sqrMagnitude < data.Range * data.Range)
            .Subscribe(_ => OnDamaged?.Invoke())
            .AddTo(transform);
    }
}
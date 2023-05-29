using UniRx;
using UnityEngine;

/// <summary>
/// �v���C���[�̗̑͂𐧌䂷��N���X
/// �G�e���Ԃ������ۂɏ������g���K�[����
/// </summary>
[System.Serializable]
public class PlayerLifePointModule
{
    [Header("�ő�̗�(�_���[�W1�̒e�ɑ΂���)")]
    [SerializeField] int _maxLifePoint = 3;

    ReactiveProperty<int> _lifePoint = new();

    public IReadOnlyReactiveProperty<int> CurrentLifePoint => _lifePoint;

    public void InitOnAwake(Transform transform)
    {
        Reset();

        MessageBroker.Default.Receive<DamageData>()
            .Where(data => transform.CompareTag(data.Tag))
            .Where(data => (data.HitPos - transform.position).sqrMagnitude < data.Range * data.Range)
            .Where(_ => _lifePoint.Value > 0)
            .Subscribe(_ => 
            {
                // �_���[�W���󂯂đ̗͂����������b�Z�[�W�𑗐M
                MessageBroker.Default.Publish(new PlayerLifePointData(_lifePoint.Value, _lifePoint.Value - 1));
                _lifePoint.Value--;
            }).AddTo(transform);
    }

    /// <summary>
    /// ���X�|�[������ۂɑ̗͂����Z�b�g����K�v������
    /// </summary>
    void Reset() => _lifePoint.Value = _maxLifePoint;
}
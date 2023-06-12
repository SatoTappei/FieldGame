using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

/// <summary>
/// �G�����F�\���ǂ����̃t���O�𐧌䂷��N���X
/// �v���C���[�Ǝ��g�̋����𑪂��Ĉ�苗���ȉ��������王�F�\�Ƃ�
/// BehaviorTree���Ŗ��t���[���̍X�V�������s����
/// </summary>
[System.Serializable]
public class EnemyActiveControlModule
{
    static readonly float Interval = 0.5f;
    static readonly float Distance = 8.0f;

    [Header("����Renderer���J�����ɉf���Ă��邩�Ŕ��肷��")]
    [SerializeField] Renderer _renderer;

    /// <summary>
    /// ���̃t���O���Q�Ƃ��ăv���C���[�Ƃ̋����ɉ������X�V���邵�Ȃ���؂�ւ���
    /// </summary>
    public bool IsActive { get; private set; } = true;

    public void InitOnAwake(Transform transform, Transform player)
    {
        // �����̓G�������^�C�~���O�ŏ�������Əd���Ȃ�̂ŊJ�n���Ԃ����炷
        System.TimeSpan due = System.TimeSpan.FromSeconds(Random.Range(0, 1.0f));

        // ���Ԋu�Ńv���C���[�Ƃ̋������v�Z���A�t���O�𑀍삷��
        Observable.Timer(due, System.TimeSpan.FromSeconds(Interval)).Subscribe(_ =>
        {
            float dist = Distance * Distance * Distance * Distance;
            IsActive = Vector3.SqrMagnitude(player.position - transform.position) < dist;
        }).AddTo(transform);
    }

    public void DrawActiveRange(Transform transform)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Distance * Distance);
    }
}
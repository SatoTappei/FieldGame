using UniRx;
using UnityEngine;

/// <summary>
/// �v���C���[�ƓG�̃_���[�W�̃f�[�^�̍\����
/// ���̍\���̂����b�Z�[�W���O�ł���肷�邱�ƂŃ_���[�W�̏������s��
/// </summary>
public struct DamageData
{
    public DamageData(Vector3 hitPos, float range, string targetTag)
    {
        HitPos = hitPos;
        Range = range;
        TargetTag = targetTag;
    }

    public Vector3 HitPos { get; }
    public float Range { get; }
    public string TargetTag { get; }
}

/// <summary>
/// �_���[�W�̏����̃��b�Z�[�W�̑��M�����b�v�����N���X
/// </summary>
public static class DamageMessageSender
{
    public static void SendMessageToPlayer(Vector3 hitPos, float range)
    {
        MessageBroker.Default.Publish(new DamageData(hitPos, range, "Player"));
    }

    public static void SendMessageToEnemy(Vector3 hitPos, float range)
    {
        MessageBroker.Default.Publish(new DamageData(hitPos, range, "Enemy"));
    }
}

using UnityEngine;

/// <summary>
/// ���̃_���[�W��^���郁�b�Z�[�W�𑗐M����N���X
/// �{�^���Ɋ��蓖�ĂĎg�p����
/// </summary>
public class TempDamageMessageSystem : MonoBehaviour
{
    public void Send()
    {
        DamageMessageSender.SendMessageToEnemy(Vector3.zero, 10000);
    }
}

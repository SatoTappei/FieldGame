using UniRx;
using UnityEngine;

/// <summary>
/// �䎌��\�����邽�߂̃��b�Z�[�W�𑗐M����N���X
/// </summary>
[System.Serializable]
public class EnemyLineModule
{
    static string[] _lines =
    {
        "���A�������߂��c",
        "���ځ[��",
        "������Ƒ҂��Ă��̂�",
        "�������",
        "�²�� �²�ު� �¸� �Ǿު-�!"
    };

    public void SendDefeatedLineMessage()
    {
        string line = _lines[Random.Range(0, _lines.Length)];
        MessageBroker.Default.Publish(new LineData("�G", line));
    }
}

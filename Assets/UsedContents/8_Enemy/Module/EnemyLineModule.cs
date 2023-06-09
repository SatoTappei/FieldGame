using UniRx;
using UnityEngine;

/// <summary>
/// �䎌��\�����邽�߂̃��b�Z�[�W�𑗐M����N���X
/// </summary>
[System.Serializable]
public class EnemyLineModule
{
    static string[] _defeatedLines =
    {
        "���A�Z�M���c�������߂��c",
        "���ځ[��",
        "������Ƒ҂��Ă��̂�",
        "���т������� ��イ���� �I",
        "�²�� �²�ު� �¸� �Ǿު-�!"
    };

    static string[] _detectLines =
    {
        "�G���m���������I",
        "�^�[�Q�b�g�����`�I",
        "�ˌ��[���I"
    };

    public void SendDetectLineMessage()
    {
        string line = _detectLines[Random.Range(0, _detectLines.Length)];
        MessageBroker.Default.Publish(new LineData("�G", line));
    }

    public void SendDefeatedLineMessage()
    {
        string line = _defeatedLines[Random.Range(0, _defeatedLines.Length)];
        MessageBroker.Default.Publish(new LineData("�G", line));
    }
}

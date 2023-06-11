using UniRx;
using UnityEngine;

/// <summary>
/// �䎌��\�����邽�߂̃��b�Z�[�W�𑗐M����N���X
/// </summary>
[System.Serializable]
public class EnemyLineModule
{
    static readonly string[] DefeatedLines =
    {
        "�u���A�Z�M���c�������߂��c�v",
        "�u���ځ[��v",
        "�u������Ƒ҂��Ă��̂ˁv",
        "�u���т������� ��イ���� �I�v",
        "�u�²�� �²�ު� �¸� �Ǿު-�!�v"
    };

    static readonly string[] DetectLines =
    {
        "�u�G���m���������I�v",
        "�u�^�[�Q�b�g�����`�I�v",
        "�u�ˌ��[���I�v"
    };

    public void SendDetectLineMessage()
    {
        string line = DetectLines[Random.Range(0, DetectLines.Length)];
        MessageBroker.Default.Publish(new LineData("�G", line));
    }

    public void SendDefeatedLineMessage()
    {
        string line = DefeatedLines[Random.Range(0, DefeatedLines.Length)];
        MessageBroker.Default.Publish(new LineData("�G", line));
    }
}

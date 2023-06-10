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
        "�u���A�Z�M���c�������߂��c�v",
        "�u���ځ[��v",
        "�u������Ƒ҂��Ă��̂ˁv",
        "�u���т������� ��イ���� �I�v",
        "�u�²�� �²�ު� �¸� �Ǿު-�!�v"
    };

    static string[] _detectLines =
    {
        "�u�G���m���������I�v",
        "�u�^�[�Q�b�g�����`�I�v",
        "�u�ˌ��[���I�v"
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

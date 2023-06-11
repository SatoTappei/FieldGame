using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

/// <summary>
/// �Q�[���̐i�s�ɉ������䎌�𑗐M����N���X
/// ���̃N���X�̃��b�Z�[�W���M�Ɠ������Ă��Ȃ��̂Ŋ��荞�܂��̂Œ��ӂ��K�v
/// </summary>
[System.Serializable]
public class InGameStateLineModule
{
    static readonly string[] InGameStartLines =
    {
        "�u�A�C�e�����W�߂ĒE�o���Ăˁv",
        "�u���΂炭������E�o�p�̃w���R�v�^�[�������v",
        "�u�撣��(���L�́M)���v",
    };

    static readonly float Interval = 2.0f;

    public async UniTaskVoid SendInGameStartLineAsync(CancellationToken token)
    {
        foreach (string line in InGameStartLines)
        {
            MessageBroker.Default.Publish(new LineData("�̂��l", line));
            await UniTask.Delay(System.TimeSpan.FromSeconds(Interval), cancellationToken: token);
        }
    }
}

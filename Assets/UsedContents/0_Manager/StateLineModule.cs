using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// �Q�[���̐i�s�ɉ������䎌�𑗐M����N���X
/// </summary>
//[System.Serializable]
//public class StateLineModule
//{
//    static readonly string[] InGameStartLines =
//    {
//        "�u�A�C�e�����W�߂ĒE�o���Ăˁv",
//        "�u���΂炭������E�o�p�̃w���R�v�^�[�������v",
//        "�u�撣��(���L�́M)���v",
//    };

//    static readonly float Delay = 2.0f;

//    /// <summary>
//    /// �G�̑䎌�̑��M�Ɋ��荞�܂��̂Ńv���C���[�̔z�u�ɍH�v���K�v
//    /// </summary>
//    public async UniTaskVoid SendInGameStartLineAsync(CancellationToken token)
//    {
//        foreach(string line in InGameStartLines)
//        {
//            MessageBroker.Default.Publish(new LineData("�̂��l", line));
//            await UniTask.Delay(System.TimeSpan.FromSeconds(Delay), cancellationToken: token);
//        }
//    }
//}

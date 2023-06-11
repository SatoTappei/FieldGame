using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

/// <summary>
/// �C���Q�[���̗���𐧌䂷��N���X
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] InGameStateLineModule _stateLineModule;
    [SerializeField] InGameTimerModule _timerModule;
    [SerializeField] InGameHelicopterMoveModule _heliMoveModule;
    [SerializeField] InGameEscapeAreaModule _escapeAreaModule;

    void Awake()
    {
        _escapeAreaModule.Inactive();

        // �^�C�g����ʂ���C���Q�[���ɑJ�ڂ������Ƃ��g���K�[����
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => 
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            Execute(token).Forget();
        }).AddTo(this);
    }

    async UniTaskVoid Execute(CancellationToken token)
    {
        // TODO:�G�̑䎌�̑��M�Ɋ��荞�܂��̂Ńv���C���[�̔z�u�ɍH�v���K�v
        _stateLineModule.SendInGameStartLineAsync(token).Forget();
        await _timerModule.Execute(token);
        await _heliMoveModule.Execute(token);
        _escapeAreaModule.Active();
        await _escapeAreaModule.Execute(token, gameObject);
        Debug.Log("�E�o");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// �C���Q�[���̗���𐧌䂷��N���X
/// </summary>
public class InGameStream : MonoBehaviour
{
    [SerializeField] InGameStateLineModule _stateLineModule;
    [SerializeField] InGameTimerModule _timerModule;

    void Awake()
    {
        _timerModule.InitOnAwake();

        bool inGameStart = false;
        // �^�C�g����ʂ���C���Q�[���ɑJ�ڂ������Ƃ��g���K�[����
        MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_ => 
        {
            // �C���Q�[���J�n�̃t���O�𗧂Ă�
            inGameStart = true;
            // �G�̑䎌�̑��M�Ɋ��荞�܂��̂Ńv���C���[�̔z�u�ɍH�v���K�v
            _stateLineModule.SendInGameStartLineAsync(this.GetCancellationTokenOnDestroy()).Forget();

            // �C���Q�[���J�n�̃t���O���o���Ă��疈�t���[���X�V�����
            this.UpdateAsObservable()
                .TakeWhile(_ => inGameStart)
                .Subscribe(_ =>
                {
                    _timerModule.Update();
                });
        }).AddTo(this);
       
    }
}

// �Q�[���J�n���Ƀ��b�Z�[�W
// ������x���Ԃ��o�߂��������������Ƃł����b�Z�[�W
// ���������瓞���������b�Z�[�W
// �E�o������N���A�A�V�[���������[�h

// �K�v: �C���Q�[���̃^�C�}�[
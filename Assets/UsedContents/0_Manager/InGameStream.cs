using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// �C���Q�[���̗���𐧌䂷��N���X
/// </summary>
//public class InGameStream : MonoBehaviour
//{
//    //[SerializeField] StateLineModule _stateLineModule;

//    void Awake()
//    {
//        // �^�C�g����ʂ���C���Q�[���ɑJ�ڂ������Ƃ��g���K�[����
//        //MessageBroker.Default.Receive<ToInGameTrigger>().Subscribe(_=>
//        //{
//        //    _stateLineModule.SendInGameStartLineAsync(this.GetCancellationTokenOnDestroy()).Forget();
//        //}).AddTo(this);
//    }
//}

// �Q�[���J�n���Ƀ��b�Z�[�W
// ������x���Ԃ��o�߂��������������Ƃł����b�Z�[�W
// ���������瓞���������b�Z�[�W
// �E�o������N���A�A�V�[���������[�h

// �K�v: �C���Q�[���̃^�C�}�[
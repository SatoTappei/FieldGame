using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// �E�o�n�_�Ƀw���R�v�^�[������Ă��鉉�o�𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class InGameHelicopterMoveModule
{
    static readonly float Speed = 6.0f;
    /// <summary>
    /// �ړI�n�ɓ��������Ƃ݂Ȃ�����
    /// ���̒l�͈ړ����x�𑬂������ꍇ�A�������Ȃ��Ƃ����Ȃ�
    /// </summary>
    static readonly float Approximately = 0.3f;

    [SerializeField] Transform _helicopter;
    [SerializeField] Transform _helicopterModel;
    [Header("�w���R�v�^�[����������ʒu")]
    [SerializeField] Transform _heliRendezvousPoint;

    public async UniTask Execute(CancellationToken token)
    {
        // TODO:�w���ƃ|�C���g��������������Ȃ��Ə㉺�΂߂���������ԂɂȂ��Ă��܂�
        Vector3 diff = _heliRendezvousPoint.position - _helicopter.position;
        _helicopterModel.forward = diff;

        while(diff.sqrMagnitude >= Approximately)
        {
            token.ThrowIfCancellationRequested();

            diff = _heliRendezvousPoint.position - _helicopter.position;
            _helicopter.Translate(diff.normalized * Time.deltaTime * Speed);
            await UniTask.Yield(token);
        }
    }
}

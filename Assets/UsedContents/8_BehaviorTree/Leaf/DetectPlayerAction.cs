using System.Buffers;
using UnityEngine;

/// <summary>
/// �v���C���[�����m����A�N�V�����m�[�h
/// </summary>
public class DetectPlayerAction : BehaviorTreeNode
{
    public DetectPlayerAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // ����1�ȏ�̔z����؂�Ă��ăv���C���[�����m����
        Collider[] hits = ArrayPool<Collider>.Shared.Rent(1);
        int count = Physics.OverlapSphereNonAlloc(BlackBoard.Transform.position,
            BlackBoard.DetectRadius, hits, BlackBoard.PlayerLayer);
        ArrayPool<Collider>.Shared.Return(hits, true);

        return count > 0 ? State.Success : State.Failure;
    }
}

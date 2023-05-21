using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�ւ̌o�H��T������A�N�V�����m�[�h
/// ���̃m�[�h�ŒT�������p�X��p���Ĉړ����s��
/// </summary>
public class PathfindingToPlayerAction : BehaviorTreeNode
{
    public PathfindingToPlayerAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // �o�H�T���̃N���X�ŋ��߂��o�H�����ɏ�������
        Queue<Vector3> path = TempPathfindingSystem.Instance.GetPath(BlackBoard.Transform.position);
        BlackBoard.Path = path;

        return State.Success;
    }
}

using UnityEngine;

/// <summary>
/// �v���C���[�ւ̌o�H��T������A�N�V�����m�[�h
/// ���̃m�[�h�ŒT�������p�X��p���Ĉړ����s��
/// </summary>
public class PathfindingToPlayerAction : BehaviorTreeNode
{
    public PathfindingToPlayerAction(string nodeName) : base(nodeName) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        // TODO:���߂��Ȃ������玸�s��Ԃ��悤�ɂ���
        Debug.Log("PathfindingToPlayerAction�Ōo�H�����߂�");
        return State.Success;
    }
}

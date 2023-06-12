using UnityEngine;

/// <summary>
/// �v���C���[�����m����A�N�V�����m�[�h
/// ���m�����ꍇ�͐����A���Ȃ������ꍇ�͎��s��Ԃ������ŁA���ɏ������݂͂��Ȃ�
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
        // �v���C���[�̌��ɏ�Q��������ꍇ�ARaycastHit�ɓ����Ă���͎̂�O�ɂ���Player�̂�
        // ��Q���̌��Ƀv���C���[������ꍇ��RaycastHit�ɏ�Q��������̂Ŏ��E�ɉf��Ȃ�
        Vector3 rayOrigin = BlackBoard.Transform.position;
        rayOrigin.y += BlackBoard.PlayerVisibleRayOffset;
        Vector3 dir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;
        bool rayHit = Physics.Raycast(rayOrigin, dir, out RaycastHit hit, 
            BlackBoard.DetectRadius, BlackBoard.PlayerDetectLayer);

        if (rayHit)
        {
            return hit.collider.CompareTag("Player") ? State.Success : State.Failure;
        }
        else
        {
            return State.Failure;
        }
    }
}
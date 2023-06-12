using UnityEngine;

public class MonkeyModelDetectPlayerAction : MonkeyModelTreeNode
{
    public MonkeyModelDetectPlayerAction(string nodeName, MonkeyModelBlackBoard blackBoard)
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
            return hit.collider.CompareTag(BlackBoard.PlayerTag) ? State.Success : State.Failure;
        }
        else
        {
            return State.Failure;
        }
    }
}

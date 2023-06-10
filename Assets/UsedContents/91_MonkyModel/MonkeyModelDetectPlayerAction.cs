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
        // プレイヤーの後ろに障害物がある場合、RaycastHitに入ってくるのは手前にあるPlayerのみ
        // 障害物の後ろにプレイヤーがいる場合はRaycastHitに障害物が入るので視界に映らない
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

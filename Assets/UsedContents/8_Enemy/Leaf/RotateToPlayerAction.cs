using UnityEngine;

/// <summary>
/// Lerpを使ってプレイヤーの方を向くように回転するアクションノード
/// </summary>
public class RotateToPlayerAction : BehaviorTreeNode
{
    public RotateToPlayerAction(string nodeName, BehaviorTreeBlackBoard blackBoard)
    : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        Vector3 dir = (BlackBoard.Player.position - BlackBoard.Transform.position).normalized;
        Quaternion playerRot = Quaternion.AngleAxis(BlackBoard.Player.position.y, Vector3.up);
        Quaternion rot = Quaternion.LookRotation(playerRot * dir, Vector3.up);
        rot.x = 0;
        rot.z = 0;
        BlackBoard.Model.rotation = Quaternion.Lerp(BlackBoard.Model.rotation, rot,
            Time.deltaTime * BlackBoard.RotSpeed);

        return State.Success;
    }
}

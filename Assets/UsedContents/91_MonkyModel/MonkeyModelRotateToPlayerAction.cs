using UnityEngine;

public class MonkeyModelRotateToPlayerAction : MonkeyModelTreeNode
{
    public MonkeyModelRotateToPlayerAction(string nodeName, MonkeyModelBlackBoard blackBoard)
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

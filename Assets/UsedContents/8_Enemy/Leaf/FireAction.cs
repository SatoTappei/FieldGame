/// <summary>
/// 攻撃を行うアクションノード
/// </summary>
public class FireAction : BehaviorTreeNode
{
    public FireAction(string nodeName, BehaviorTreeBlackBoard blackBoard) 
        : base(nodeName, blackBoard) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        BlackBoard.FireParticle.Play();
        return State.Success;
    }
}

/// <summary>
/// ルートノードのクラス
/// </summary>
public class RootNode : BehaviorTreeNode
{
    public BehaviorTreeNode _child;

    public RootNode() : base("ルートノード") { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        return _child.Update();
    }
}

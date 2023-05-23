/// <summary>
/// ���[�g�m�[�h�̃N���X
/// </summary>
public class RootNode : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;

    public RootNode() : base("���[�g�m�[�h") { }

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

    public void AddChild(BehaviorTreeNode node) => _child = node;
}

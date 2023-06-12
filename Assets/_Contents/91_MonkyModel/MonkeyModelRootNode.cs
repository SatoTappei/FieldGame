public class MonkeyModelRootNode : MonkeyModelTreeNode, IMonkeyModelTreeNodeHolder
{
    MonkeyModelTreeNode _child;

    public MonkeyModelRootNode() : base("���[�g�m�[�h") { }

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

    public void AddChild(MonkeyModelTreeNode node) => _child = node;
}

public class MonkeyModelLoopDecorator : MonkeyModelTreeNode, IMonkeyModelTreeNodeHolder
{
    MonkeyModelTreeNode _child;

    public MonkeyModelLoopDecorator(string nodeName) : base(nodeName) { }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        State result = _child.Update();

        if (result == State.Failure)
        {
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }

    public void AddChild(MonkeyModelTreeNode node) => _child = node;
}

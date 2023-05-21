/// <summary>
/// 子が失敗するまで子を実行しつつ実行中を返すデコレーターノードのクラス
/// </summary>
public class LoopDecorator : BehaviorTreeNode, IBehaviorTreeNodeHolder
{
    BehaviorTreeNode _child;

    public LoopDecorator(string nodeName) : base(nodeName) { }

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

    public void AddChild(BehaviorTreeNode node) => _child = node;
}
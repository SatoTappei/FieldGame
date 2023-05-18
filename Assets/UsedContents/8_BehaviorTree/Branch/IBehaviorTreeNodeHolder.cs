/// <summary>
/// BehaviorTreeで使用するノードをクラス内に保持する処理を
/// 実装させるインターフェース
/// </summary>
public interface IBehaviorTreeNodeHolder
{
    public void AddChild(BehaviorTreeNode node);
}

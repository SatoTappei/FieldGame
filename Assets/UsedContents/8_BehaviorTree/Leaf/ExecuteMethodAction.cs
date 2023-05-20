using UnityEngine.Events;

// 諸々の処理をこっちに書きたいので未使用

/// <summary>
/// メソッドを実行するだけのアクションノード
/// </summary>
public class ExecuteMethodAction : BehaviorTreeNode
{
    UnityAction _action;

    public ExecuteMethodAction(UnityAction action, string nodeName) : base(nodeName)
    {
        _action = action;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        _action?.Invoke();
        return State.Success;
    }
}

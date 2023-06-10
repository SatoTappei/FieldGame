using UnityEngine;
using UnityEngine.Events;

public class MonkeyModelFireAction : MonkeyModelTreeNode
{
    public MonkeyModelFireAction(string nodeName, MonkeyModelBlackBoard blackBoard)
            : base(nodeName, blackBoard) { }

    public UnityEvent OnFire;

    protected override void OnEnter()
    {
        if (OnFire == null && BlackBoard.DebugLog)
        {
            Debug.LogWarning("攻撃用のメソッドが登録されていないので何も処理を実行しない");
        }
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        OnFire.Invoke();
        return State.Success;
    }
}

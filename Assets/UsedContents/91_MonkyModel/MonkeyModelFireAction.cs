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
            Debug.LogWarning("�U���p�̃��\�b�h���o�^����Ă��Ȃ��̂ŉ������������s���Ȃ�");
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

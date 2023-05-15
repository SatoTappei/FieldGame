using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// セレクターノードのクラス
/// </summary>
public class SelectorNode : BehaviorTreeNode
{
    // ノードの選択方法
    //  ランダムに1つ選ぶ
    //  順番に選ぶ

    // 子のノードの中から1つを実行して成功したらSuccessを返す
    // 選択した子が失敗した場合は別の子を実行する
    // 全ての子が失敗を返した場合は失敗を返す
    // 攻撃もしくは回復もしくは…みたいな場合に使う

    List<BehaviorTreeNode> _childList = new();
    int _currentChildIndex;

    protected override void OnEnter()
    {
        _currentChildIndex = 0;
        Debug.Log("Selector開始");
    }

    protected override void OnExit()
    {
        Debug.Log("Selector終了");
    }

    protected override State OnStay()
    {
        // 毎フレーム子の結果を返してもらう
        State result = _childList[_currentChildIndex].Update();

        // 最後の子が失敗を返したらSelector自体が失敗を返す
        if (_currentChildIndex == _childList.Count && result == State.Failure)
        {
            return State.Failure;
        }

        // 子が成功を返したら次の子を実行する
        if (result == State.Success)
        {
            return State.Success;
        }
        // 子が失敗を返したら次の子を試す
        else if (result == State.Failure)
        {
            _currentChildIndex++;
        }

        return State.Runnning;
    }

    public void AddChild(BehaviorTreeNode node) => _childList.Add(node);
}

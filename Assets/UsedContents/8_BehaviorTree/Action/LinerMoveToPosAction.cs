using UnityEngine;

/// <summary>
/// 指定した座標に向けて移動するアクションノード
/// 壁や敵を無視して直線移動を行う
/// </summary>
public class LinerMoveToPosAction : BehaviorTreeNode
{
    /// <summary>
    /// 目的地に到着したとみなす距離
    /// </summary>
    static readonly float Approximately = 0.05f;

    Transform _transform;
    Vector3 _targetPos;
    float _speed;

    public LinerMoveToPosAction(Collider collider, Transform transform,
        Vector3 targetPos, float speed) : base(collider)
    {
        _transform = transform;
        _targetPos = targetPos;
        _speed = speed;
    }

    protected override void OnEnter()
    {
        Debug.Log(_targetPos + "への移動開始");
    }

    protected override void OnExit()
    {
        Debug.Log(_targetPos + "への移動終了");
    }

    protected override State OnStay()
    {
        if (IsTriggerEnter)
        {
            Debug.Log("ヒット!");
        }

        if (Vector3.Distance(_transform.position, _targetPos) <= Approximately)
        {
            return State.Success;
        }
        else
        {
            float speed = Time.deltaTime * _speed;
            _transform.position = Vector3.MoveTowards(_transform.position, _targetPos, speed);

            return State.Runnning;
        }
    }
}

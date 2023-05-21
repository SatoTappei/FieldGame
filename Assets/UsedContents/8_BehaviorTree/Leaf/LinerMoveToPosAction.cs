using UnityEngine;

/// <summary>
/// 指定した座標に向けて移動するアクションノード
/// </summary>
public class LinerMoveToPosAction : BehaviorTreeNode
{
    /// <summary>
    /// 目的地に到着したとみなす距離
    /// この値は移動速度を速くした場合、調整しないといけない
    /// </summary>
    static readonly float Approximately = 0.3f;

    Rigidbody _rigidbody;
    Transform _transform;
    Transform _model;
    Vector3 _targetPos;
    float _speed;

    // TODO:BlackBoardにデータをまとめたのでBlackBoardを渡すだけで大丈夫
    public LinerMoveToPosAction(Rigidbody rigidbody, Vector3 targetPos, float speed,
        string nodeName) : base(nodeName)
    {
        _rigidbody = rigidbody;
        _transform = rigidbody.GetComponent<Transform>();
        _model = rigidbody.transform.GetChild(0);
        if (_model.name != "Model")
        {
            Debug.LogError("回転させるためのModelが1番目の子になっていない、もしくは名前が違う");
        }
        _targetPos = targetPos;
        _speed = speed;
    }

    protected override void OnEnter()
    {
    }

    protected override void OnExit()
    {
    }

    protected override State OnStay()
    {
        if ((_transform.position - _targetPos).sqrMagnitude <= Approximately)
        {
            return State.Success;
        }
        else
        {
            // 移動
            Vector3 velo = (_targetPos - _transform.position).normalized * _speed;
            velo.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velo;

            // Y軸を回転軸とした回転
            Quaternion rot = Quaternion.LookRotation(velo, Vector3.up);
            //_model <- 移動方向に回転させる


            return State.Running;
        }
    }
}

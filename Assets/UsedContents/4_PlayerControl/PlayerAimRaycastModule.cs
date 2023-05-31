using UnityEngine;

/// <summary>
/// ADSをした時の照準を表示するクラス
/// </summary>
[System.Serializable]
public class PlayerAimRaycastModule
{
    /// <summary>
    /// 照準を合わせるためのRayを飛ばす間隔
    /// </summary>
    static readonly float Interval = 0.05f;

    [SerializeField] Transform _muzzle;
    [SerializeField] Transform _model;
    [Header("AimSightCanvasの子になっているAim")]
    [SerializeField] PlayerAimSight _aimSight;
    [Header("敵と障害物のレイヤー")]
    [SerializeField] LayerMask _layerMask;
    [Header("飛ばすSphereCastの設定")]
    [SerializeField] float _distance = 50;
    [SerializeField] float _radius = 3.0f;
    [SerializeField] bool _drawGizmos = true;

    float _timer;

    public void Update(CameraMode mode)
    {
        // 一定間隔でRayを飛ばす
        _timer += Time.deltaTime;
        if (_timer > Interval)
        {
            _timer = 0;
        }
        else
        {
            return;
        }

        // Freelookの場合は照準を非表示にするだけ
        if (mode == CameraMode.Freelook)
        {
            _aimSight.Inactive();
            return;
        }
        else
        {
            _aimSight.Active();
        }

        // Rayを飛ばして照準の位置を更新する
        if (Physics.SphereCast(_muzzle.position, _radius, _model.forward, out RaycastHit hit, _distance, _layerMask))
        {
            _aimSight.SetPos(hit.point);

            if (hit.collider.CompareTag("Enemy"))
            {
                _aimSight.SetEnemyOverlapColor();
            }
            else
            {
                _aimSight.SetDefaultColor();
            }
        }
        else
        {
            _aimSight.SetPos(_muzzle.position + _model.forward * _distance);
            _aimSight.SetDefaultColor();
        }
    }

    public void Visualize()
    {
        if (!_drawGizmos) return;

        Gizmos.DrawRay(_muzzle.position, _model.forward * _distance);
        Gizmos.DrawWireSphere(_muzzle.position + _model.forward * _distance, _radius);
    }
}
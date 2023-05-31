using UnityEngine;

/// <summary>
/// ADS���������̏Ə���\������N���X
/// </summary>
[System.Serializable]
public class PlayerAimRaycastModule
{
    /// <summary>
    /// �Ə������킹�邽�߂�Ray���΂��Ԋu
    /// </summary>
    static readonly float Interval = 0.05f;

    [SerializeField] Transform _muzzle;
    [SerializeField] Transform _model;
    [Header("AimSightCanvas�̎q�ɂȂ��Ă���Aim")]
    [SerializeField] PlayerAimSight _aimSight;
    [Header("�G�Ə�Q���̃��C���[")]
    [SerializeField] LayerMask _layerMask;
    [Header("��΂�SphereCast�̐ݒ�")]
    [SerializeField] float _distance = 50;
    [SerializeField] float _radius = 3.0f;
    [SerializeField] bool _drawGizmos = true;

    float _timer;

    public void Update(CameraMode mode)
    {
        // ���Ԋu��Ray���΂�
        _timer += Time.deltaTime;
        if (_timer > Interval)
        {
            _timer = 0;
        }
        else
        {
            return;
        }

        // Freelook�̏ꍇ�͏Ə����\���ɂ��邾��
        if (mode == CameraMode.Freelook)
        {
            _aimSight.Inactive();
            return;
        }
        else
        {
            _aimSight.Active();
        }

        // Ray���΂��ďƏ��̈ʒu���X�V����
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
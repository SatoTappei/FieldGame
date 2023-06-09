using UnityEngine;

/// <summary>
/// ���o�p��Particle�̍Đ����s���N���X
/// </summary>
[System.Serializable]
public class EnemyPerformanceModule
{
    [SerializeField] GameObject _defeatedParticle;

    public void Defeated(Vector3 pos)
    {
        Object.Instantiate(_defeatedParticle, pos, Quaternion.identity);
    }
}

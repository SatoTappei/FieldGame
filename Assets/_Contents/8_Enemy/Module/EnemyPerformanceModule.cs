using UnityEngine;

/// <summary>
/// 演出用のParticleの再生を行うクラス
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

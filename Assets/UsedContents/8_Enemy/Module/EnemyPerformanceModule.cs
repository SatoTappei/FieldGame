using UnityEngine;

// TODO:演出はECSが絡むのでこのクラスは大幅に変更するかもしれない

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

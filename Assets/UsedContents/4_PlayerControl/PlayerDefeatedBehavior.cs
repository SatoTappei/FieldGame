using UnityEngine;

/// <summary>
/// プレイヤーが死んだ際の処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerDefeatedBehavior
{
    [SerializeField] Transform _respawnPoint;

    public void Respawn(Transform transform)
    {
        transform.position = _respawnPoint.position;
    }
}

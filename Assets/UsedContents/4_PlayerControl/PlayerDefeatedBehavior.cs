using UnityEngine;

/// <summary>
/// �v���C���[�����񂾍ۂ̏������s���N���X
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

using UnityEngine;

// TODO:���o��ECS�����ނ̂ł��̃N���X�͑啝�ɕύX���邩������Ȃ�

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

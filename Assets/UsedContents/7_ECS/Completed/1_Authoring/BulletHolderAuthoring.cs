using Unity.Entities;
using UnityEngine;

/// <summary>
/// �G�̒e��Entity������Entity�̃N���X
/// �v���C���[�̓��͂ɂ���Ă��̃N���X�����e��Entity�����˂����
/// </summary>
public class BulletHolderAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _playerBulletPrefab;
    [SerializeField] GameObject _enemnyBulletPrefab;
    [SerializeField] uint _randomSeed;

    public GameObject Prefab => _playerBulletPrefab;

    class Baker : Baker<BulletHolderAuthoring>
    {
        public override void Bake(BulletHolderAuthoring authoring)
        {
            AddComponent(new BulletHolderComponent 
            {
                _playerPrototype = GetEntity(authoring.Prefab),
                _enemyPrototype = GetEntity(authoring._enemnyBulletPrefab),
            });
            AddComponent(new RandomValueComponent 
            { 
                _value = Unity.Mathematics.Random.CreateFromIndex(authoring._randomSeed),
                _seed = authoring._randomSeed
            });
        }
    }
}
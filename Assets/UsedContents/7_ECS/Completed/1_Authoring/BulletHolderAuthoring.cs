using Unity.Entities;
using UnityEngine;

/// <summary>
/// �G�̒e��Entity������Entity�̃N���X
/// �v���C���[�̓��͂ɂ���Ă��̃N���X�����e��Entity�����˂����
/// </summary>
public class BulletHolderAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab;
    [SerializeField] uint _randomSeed;

    public GameObject Prefab => _prefab;

    class Baker : Baker<BulletHolderAuthoring>
    {
        public override void Bake(BulletHolderAuthoring authoring)
        {
            AddComponent(new BulletHolderComponent { _prototype = GetEntity(authoring.Prefab) });
            AddComponent(new RandomValueComponent 
            { 
                _value = Unity.Mathematics.Random.CreateFromIndex(authoring._randomSeed),
                _seed = authoring._randomSeed
            });
        }
    }
}
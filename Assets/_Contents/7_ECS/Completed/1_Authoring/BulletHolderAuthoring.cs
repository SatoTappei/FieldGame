using Unity.Entities;
using UnityEngine;

/// <summary>
/// 敵の弾のEntityを持つEntityのクラス
/// プレイヤーの入力によってこのクラスが持つ弾のEntityが発射される
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
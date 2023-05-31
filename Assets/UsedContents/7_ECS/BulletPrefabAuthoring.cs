using UnityEngine;
using Unity.Entities;

/// <summary>
/// ’e‚ÌPrefab‚ðEntity‚É•ÏŠ·‚·‚éAuthoringƒNƒ‰ƒX
/// </summary>
public class BulletPrefabAuthoring : MonoBehaviour
{
    [SerializeField] GameObject _prefab;

    public GameObject Prefab => _prefab;
}

public class BulletPrefabBaker : Baker<BulletPrefabAuthoring>
{
    public override void Bake(BulletPrefabAuthoring authoring)
    {
        AddComponent(new BulletPrefabComponent
        {
            _value = GetEntity(authoring.Prefab),
        });
        AddComponent(new BulletSpeedComponent
        {
            _value = 3.0f,
        });
    }
}
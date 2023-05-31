using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] float3 _dir;

    public float Speed => _speed;
    public float3 Dir => _dir;
}

public class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        AddComponent(new BulletSpeedComponent() { _value = authoring.Speed });
        AddComponent(new BulletDirectionComponent() { _value = authoring.Dir });
    }
}
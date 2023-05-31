using Unity.Entities;
using Unity.Mathematics;

public struct BulletSpeedComponent : IComponentData
{
    public float _value;
}

public struct BulletDirectionComponent : IComponentData
{
    public float3 _value;
}

public struct BulletPrefabComponent : IComponentData
{
    public Entity _value;
}
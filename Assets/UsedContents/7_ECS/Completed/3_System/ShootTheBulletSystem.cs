using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// MonoBehaviour側の入力された値に応じて弾を発射するSystem
/// </summary>
[BurstCompile]
[UpdateAfter(typeof(TriggerByMonoSystem))]
public partial struct ShootTheBulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BulletHolderComponent>();
        state.RequireForUpdate<RandomValueComponent>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PlayerBulletSpawnComponent component = SystemAPI.GetSingleton<PlayerBulletSpawnComponent>();
        if (component._active)
        {
            // 弾を生成
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
            BulletHolderComponent holder = SystemAPI.GetSingleton<BulletHolderComponent>();

            for (int i = 0; i < 100; i++)
            {
                Entity entity = ecb.Instantiate(i, holder._prototype);
                ecb.AddComponent(i, entity, new LocalToWorldTransform { Value = GetTransform(component._pos) });
                ecb.AddComponent(i, entity, new BulletSpeedComponent { _value = 60.0f });
                ecb.AddComponent(i, entity, new BulletDirectionComponent { _value = GetRandomDir(component._dir) });
            }

            #region Jobで実行するVer
            //var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            //new ShootTheBulletJob
            //{
            //    _ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            //    _pos = component._pos,
            //    _dir = component._dir,
            //}.Run();
            #endregion

            // 弾生成データのリセット
            component._active = false;
            component._pos = float3.zero;
            component._dir = float3.zero;
            SystemAPI.SetSingleton(component);
        }
    }

    UniformScaleTransform GetTransform(float3 pos)
    {
        return new UniformScaleTransform
        {
            Position = pos,
            Rotation = quaternion.identity,
            Scale = 0.5f,
        };
    }

    float3 GetRandomDir(float3 dir)
    {
        RandomValueComponent random = SystemAPI.GetSingleton<RandomValueComponent>();

        float rx = random._value.NextFloat(-0.25f, 0.25f);
        float ry = random._value.NextFloat(-0.25f, 0.25f);
        float rz = random._value.NextFloat(-0.25f, 0.25f);
        dir.x += rx;
        dir.y += ry;
        dir.z += rz;

        random._seed++;
        random._value = Random.CreateFromIndex(random._seed);
        SystemAPI.SetSingleton(random);

        return dir;
    }
}

#region Run()で実行するJob
//[BurstCompile]
//public partial struct ShootTheBulletJob : IJobEntity
//{
//    public EntityCommandBuffer _ecb;
//    public float3 _pos;
//    public float3 _dir;

//    void Execute(BulletHolderComponent holder)
//    {
//        Entity entity = _ecb.Instantiate(holder._prototype);

//        // SetComponentで追加しようとするとエラーが出る
//        _ecb.AddComponent(entity, new LocalToWorldTransform { Value = GetTransform(_pos) });
//        _ecb.AddComponent(entity, new BulletSpeedComponent { _value = 15.0f });
//        _ecb.AddComponent(entity, new BulletDirectionComponent { _value = _dir });
//    }

//    /// <summary>
//    /// 座標に応じた回転と大きさがデフォルトのTransformを取得する
//    /// </summary>
//    UniformScaleTransform GetTransform(float3 pos)
//    {
//        return new UniformScaleTransform
//        {
//            Position = pos,
//            Rotation = quaternion.identity,
//            Scale = 1.0f,
//        };
//    }
//}
#endregion
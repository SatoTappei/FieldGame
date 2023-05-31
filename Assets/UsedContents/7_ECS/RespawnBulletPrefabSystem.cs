using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Burst;
using Unity.Collections;
using Unity.Transforms;

//[BurstCompile]
//public partial struct RespawnBulletPrefabSystem : ISystem
//{
//    EntityQuery _query;

//    [BurstCompile]
//    public void OnCreate(ref SystemState state)
//    {
//        state.RequireForUpdate<BulletPrefabComponent>();
//        _query = SystemAPI.QueryBuilder().Build();
//    }

//    [BurstCompile]
//    public void OnDestroy(ref SystemState state)
//    {
//    }

//    [BurstCompile]
//    public void OnUpdate(ref SystemState state)
//    {
//        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
//        new RespawnPrefabJob
//        {
//            _ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged),
//            _prefabs = _query.ToComponentDataArray<BulletPrefabComponent>(Allocator.TempJob),
//        };
//    }
//}

//[BurstCompile]
//public partial struct RespawnPrefabJob : IJobEntity
//{
//    public EntityCommandBuffer _ecb;

//    // 読み取り専用属性、スレッドセーフになる
//    // 下はJobが終わったら自動的に解放される属性
//    // 全てのBulletPrefabComponentを持つEntityのNativeArray
//    [ReadOnly]
//    [DeallocateOnJobCompletion]
//    public NativeArray<BulletPrefabComponent> _prefabs;

//    [BurstCompile]
//    public void Execute(in Entity entity, in BulletPrefabComponent prefabData)
//    {
//        Entity prefab = Entity.Null;

//        // 配列の中を線形探索して一致するものを見つける
//        // 大量に出すとそれに応じて計算量が爆増する？
//        for(int i = 0; i < _prefabs.Length; i++)
//        {
//            if (_prefabs[i]._type == prefabData._type)
//            {
//                prefab = _prefabs[i]._entity;
//                break;
//            }
//        }

//        // この分岐に入る場合はそもそも不正な値が入っているのでおかしい
//        if(prefab == Entity.Null)
//        {
//            _ecb.DestroyEntity(entity);
//            return;
//        }

//        Entity newObject = _ecb.Instantiate(prefab);
//        _ecb.SetComponent(newObject, new LocalToWorldTransform()); // ここ違う
//        _ecb.DestroyEntity(entity);
//        //_ecb.SetComponent(newObject, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
//    }
//}
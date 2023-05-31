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

//    // �ǂݎ���p�����A�X���b�h�Z�[�t�ɂȂ�
//    // ����Job���I������玩���I�ɉ������鑮��
//    // �S�Ă�BulletPrefabComponent������Entity��NativeArray
//    [ReadOnly]
//    [DeallocateOnJobCompletion]
//    public NativeArray<BulletPrefabComponent> _prefabs;

//    [BurstCompile]
//    public void Execute(in Entity entity, in BulletPrefabComponent prefabData)
//    {
//        Entity prefab = Entity.Null;

//        // �z��̒�����`�T�����Ĉ�v������̂�������
//        // ��ʂɏo���Ƃ���ɉ����Čv�Z�ʂ���������H
//        for(int i = 0; i < _prefabs.Length; i++)
//        {
//            if (_prefabs[i]._type == prefabData._type)
//            {
//                prefab = _prefabs[i]._entity;
//                break;
//            }
//        }

//        // ���̕���ɓ���ꍇ�͂��������s���Ȓl�������Ă���̂ł�������
//        if(prefab == Entity.Null)
//        {
//            _ecb.DestroyEntity(entity);
//            return;
//        }

//        Entity newObject = _ecb.Instantiate(prefab);
//        _ecb.SetComponent(newObject, new LocalToWorldTransform()); // �����Ⴄ
//        _ecb.DestroyEntity(entity);
//        //_ecb.SetComponent(newObject, LocalTransform.FromPosition(spawner.ValueRO.SpawnPosition));
//    }
//}
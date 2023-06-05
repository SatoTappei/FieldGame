using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

/// <summary>
/// 押したらEntityを再生成するボタンのクラス
/// </summary>
//public class BulletEntityFireButton : MonoBehaviour
//{
//    public void Fire()
//    {
//        if (World.DefaultGameObjectInjectionWorld == null ||
//           World.DefaultGameObjectInjectionWorld.EntityManager == null)
//        {
//            Debug.LogError("EntityのDefaultのWorldが見つからない");
//            return;
//        }

//        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
//        Entity entity = manager.CreateEntity();

//        manager.AddComponentData(entity, new LocalToWorldTransform { });
//        manager.AddComponentData(entity, new BulletSpeedComponent { _value = 3.0f });
//        manager.AddComponentData(entity, new BulletDirectionComponent { _value = new Vector3(0, 0, 1) });
//    }
//}

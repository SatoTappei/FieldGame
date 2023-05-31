using Unity.Entities;
using UnityEngine;

/// <summary>
/// 押したらEntityを再生成するボタンのクラス
/// </summary>
//public class EntityRespawnButton : MonoBehaviour
//{
//    public void Respawn()
//    {
//        if(World.DefaultGameObjectInjectionWorld == null || 
//           World.DefaultGameObjectInjectionWorld.EntityManager == null)
//        {
//            Debug.LogError("EntityのDefaultのWorldが見つからない");
//            return;
//        }

//        EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;
//        Entity entity = manager.CreateEntity();

//        //manager.AddComponentData(entity, new BulletPrefabComponent
//        //{
//        //    _type = BulletPrefabComponent.Type.Player,
//        //});
//    }
//}

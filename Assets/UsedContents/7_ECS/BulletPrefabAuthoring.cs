using UnityEngine;
using Unity.Entities;

/// <summary>
/// 弾のPrefabをEntityに変換するAuthoringクラス
/// 空のオブジェクトにアタッチして、フィールドとして持つGameObjectを変換する
/// 自身は弾ではない
/// </summary>
//public class BulletPrefabAuthoring : MonoBehaviour
//{
//    [SerializeField] GameObject _prefab;
//    [SerializeField] float _speed = 5.0f;

//    public GameObject Prefab => _prefab;
//    public float Speed => _speed;
//}

//public class BulletPrefabBaker : Baker<BulletPrefabAuthoring>
//{
//    public override void Bake(BulletPrefabAuthoring authoring)
//    {
//        AddComponent(new BulletPrefabComponent { _value = GetEntity(authoring.Prefab) });
//        AddComponent(new BulletSpeedComponent { _value = authoring.Speed });
//    }
//}
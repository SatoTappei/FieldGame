using UnityEngine;
using Unity.Entities;

/// <summary>
/// �e��Prefab��Entity�ɕϊ�����Authoring�N���X
/// ��̃I�u�W�F�N�g�ɃA�^�b�`���āA�t�B�[���h�Ƃ��Ď���GameObject��ϊ�����
/// ���g�͒e�ł͂Ȃ�
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
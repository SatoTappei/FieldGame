using UnityEngine;
using Unity.Entities;

/// <summary>
/// �e��Prefab��Entity�ɕϊ�����Authoring�N���X
/// �e��GameObject�ɃA�^�b�`���Ď��g��ϊ�����
/// </summary>
//public class BulletPrefabAuthoringType2 : MonoBehaviour
//{
//    [SerializeField] Vector3 _dir;
//    [SerializeField] float _speed;

//    public float Speed => _speed;
//    public Vector3 NormalizedDir => _dir.normalized;
//}

//public class BulletPrefabAuthoringType2Baker : Baker<BulletPrefabAuthoringType2>
//{
//    public override void Bake(BulletPrefabAuthoringType2 authoring)
//    {
//        AddComponent(new BulletSpeedComponent { _value = authoring.Speed });
//        AddComponent(new BulletDirectionComponent { _value = authoring.NormalizedDir });
//    }
//}
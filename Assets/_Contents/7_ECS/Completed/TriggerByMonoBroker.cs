using System.Collections.Generic;
using UnityEngine;
using BulletType = ShootData.BulletType;

/// <summary>
/// ���˂����e�̏��̍\����
/// ���b�Z�[�W���O�p�ł͂Ȃ��AECS���ɓn���������b�v���邽�߂Ɏg��
/// </summary>
public struct ShootData
{
    public enum BulletType
    {
        Player,
        Enemy,
    }

    public ShootData(BulletType type, Vector3 pos, Vector3 dir)
    {
        Type = type;
        Pos = pos;
        Dir = dir;
    }

    public BulletType Type { get; private set; }
    public Vector3 Pos { get; private set; }
    public Vector3 Dir { get; private set; }
}

/// <summary>
/// GameObject����ECS���̒�����ƂȂ�N���X
/// </summary>
public class TriggerByMonoBroker : MonoBehaviour
{
    public static TriggerByMonoBroker Instance { get; private set; }

    Queue<ShootData> _queue;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _queue = new();
        }
        else
        {
            Destroy(this);
        }
    }

    public void AddShootData(BulletType type, Vector3 pos, Vector3 dir)
    {
        _queue.Enqueue(new ShootData(type, pos, dir));
    }

    public bool TryGetShootData(out ShootData data)
    {
        return _queue.TryDequeue(out data);
    }
}

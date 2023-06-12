using System.Collections.Generic;
using UnityEngine;
using BulletType = ShootData.BulletType;

/// <summary>
/// 発射した弾の情報の構造体
/// メッセージング用ではなく、ECS側に渡す情報をラップするために使う
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
/// GameObject側とECS側の仲介役となるクラス
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

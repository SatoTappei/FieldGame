using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 発射した弾の情報の構造体
/// メッセージング用ではなく、ECS側に渡す情報をラップするために使う
/// </summary>
public struct ShootData
{
    public ShootData(Vector3 pos, Vector3 dir)
    {
        Pos = pos;
        Dir = dir;
    }

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

    // テスト用:ボタンに割り当てる
    public void TestFire()
    {
        AddShootData(Vector3.zero, Vector3.forward);
    }

    public void AddShootData(Vector3 pos, Vector3 dir)
    {
        _queue.Enqueue(new ShootData(pos, dir));
    }

    // TODO:1フレームに1つ取り出すだと、大量にキューイングしてあった場合に判定と位置がズレる
    public bool TryGetShootData(out ShootData data)
    {
        return _queue.TryDequeue(out data);
    }
}

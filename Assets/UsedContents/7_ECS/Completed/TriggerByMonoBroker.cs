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

    // テスト用:そのままでも動作に影響は無し
    [SerializeField] Transform[] _muzzles;

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
    //          そのままでも動作に影響は無し
    public void TestFire()
    {
        for(int i = 0; i < 100; i++)
        {
            int r = Random.Range(0, _muzzles.Length);
            Vector3 pos = _muzzles[r].position;

            float x = Random.Range(-0.25f, 0.25f);
            float y = Random.Range(-0.25f, 0.25f);
            Vector3 normalizedDir = new Vector3(x, y, 1.0f).normalized;

            AddShootData(pos, normalizedDir);
        }
    }

    /// <summary>
    /// 大量に出すためのデバッグ用の発射
    /// Player側はこのメソッドを呼ぶだけ
    /// </summary>
    public void DebugAddShootData(Vector3 pos, Vector3 dir)
    {
        for (int i = 0; i < 100; i++)
        {
            float x = Random.Range(-0.25f, 0.25f);
            float y = Random.Range(-0.25f, 0.25f);
            float z = Random.Range(-0.25f, 0.25f);
            Vector3 normalizedDir = new Vector3(dir.x + x, dir.y + y, dir.z + z).normalized;

            AddShootData(pos, normalizedDir);
        }
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

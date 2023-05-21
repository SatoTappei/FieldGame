using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 仮の経路探索クラス
/// プレイヤーへの経路を探索する
/// </summary>
public class TempPathfindingSystem : MonoBehaviour
{
    public static TempPathfindingSystem Instance { get; private set; }

    [SerializeField] Transform _player;

    void Awake()
    {
        Instance = this;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    void Release() => Instance = null;

    public Queue<Vector3> GetPath(Vector3 currentPos)
    {
        // キューの中身通りに動くとプレイヤーに一直線に向かっていく
        Queue<Vector3> queue = new();
        queue.Enqueue(currentPos);
        queue.Enqueue(_player.position);

        return queue;
    }
}

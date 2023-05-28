using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 壁や敵などを無視して真っ直ぐ進む経路を返すクラス
/// </summary>
public class LinerPathSystem : IPathfindingSystem
{
    Transform _player;

    public LinerPathSystem(Transform player)
    {
        _player = player;
    }

    public Stack<Vector3> GetPath(Vector3 pos)
    {
        Stack<Vector3> queue = new();
        queue.Push(_player.position);
        queue.Push(pos);

        return queue;
    }
}

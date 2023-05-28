using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ǂ�G�Ȃǂ𖳎����Đ^�������i�ތo�H��Ԃ��N���X
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

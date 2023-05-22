using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̌o�H�T���N���X
/// �v���C���[�ւ̌o�H��T������
/// </summary>
public class TempPathfindingSystem : MonoBehaviour
{
    public static TempPathfindingSystem Instance { get; private set; }

    [SerializeField] Transform _player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public Queue<Vector3> GetPath(Vector3 currentPos)
    {
        // �L���[�̒��g�ʂ�ɓ����ƃv���C���[�Ɉ꒼���Ɍ������Ă���
        Queue<Vector3> queue = new();
        queue.Enqueue(currentPos);
        queue.Enqueue(_player.position);

        return queue;
    }
}

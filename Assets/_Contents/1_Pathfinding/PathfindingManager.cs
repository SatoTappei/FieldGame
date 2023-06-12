using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T���̋@�\���g���N���X
/// �e�G���o�H�T��������ۂɎg�p���邻�ꂼ��̃A���S���Y�����Ǘ�����
/// </summary>
[RequireComponent(typeof(AStarSystem))]
public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance { get; private set; }

    [SerializeField] Transform _player;

    LinerPathSystem _linerPathSystem;
    AStarSystem _aStarSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _aStarSystem = GetComponent<AStarSystem>();
            _linerPathSystem = new(_player);
        }
        else
        {
            Destroy(this);
        }
    }

    public Stack<Vector3> GetPath(Vector3 pos)
    {
        return _aStarSystem.GetPath(pos);
    }
}

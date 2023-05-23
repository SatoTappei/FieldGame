using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// A*��p�����o�H�T�����s���N���X
/// </summary>
public class PathfindingSystem : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] PathfindingGrid _pathfindingGrid;

    PathfindingTask pathfindingTask = new();
    Queue<Vector3> _path = new();

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _pathfindingGrid.InitOnStart();

        // ���t���[���v���C���[�𒆐S�ɃO���b�h�𐶐�����
        this.UpdateAsObservable().Subscribe(_ => _pathfindingGrid.Create(_player));
    }

    /// <summary>
    /// �v���C���[�ւ̌o�H��T���������Ƃ��ɊO������Ăяo�����\�b�h
    /// </summary>
    public Queue<Vector3> GetPath(Vector3 pos)
    {
        PathfindingNode toNode = _pathfindingGrid.GetNode(_player.position);
        PathfindingNode fromNode = _pathfindingGrid.GetNode(pos);
        _path = pathfindingTask.Execute(toNode, fromNode, _pathfindingGrid.Grid);

        if (_path == null)
        {
            Debug.LogWarning("�v���C���[�ւ̌o�H�̒T���Ɏ��s: " + pos);
        }

        return _path;
    }

    public void OnDrawGizmos()
    {
        if (_path != null)
        {
            foreach(Vector3 pos in _path)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }

        _pathfindingGrid.Visualize();
    }
}

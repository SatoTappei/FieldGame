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
    Stack<Vector3> _path = new();

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
    public Stack<Vector3> GetPath(Vector3 pos)
    {
#if UNITY_EDITOR
        System.Diagnostics.Stopwatch stopWatch = new();
        stopWatch.Start();
#endif

        PathfindingNode toNode = _pathfindingGrid.GetNode(_player.position);
        PathfindingNode fromNode = _pathfindingGrid.GetNode(pos);

        if (toNode == null || fromNode == null) return null;

        _path = pathfindingTask.Execute(toNode, fromNode, _pathfindingGrid.Grid);

#if UNITY_EDITOR
        stopWatch.Stop();
        Debug.Log($"�o�H�T���ɂ�����������: {stopWatch.Elapsed} ms");
#endif

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

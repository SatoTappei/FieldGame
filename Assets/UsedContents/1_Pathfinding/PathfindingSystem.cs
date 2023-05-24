using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// A*を用いた経路探索を行うクラス
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

        // 毎フレームプレイヤーを中心にグリッドを生成する
        this.UpdateAsObservable().Subscribe(_ => _pathfindingGrid.Create(_player));
    }

    /// <summary>
    /// プレイヤーへの経路を探索したいときに外部から呼び出すメソッド
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
        Debug.Log($"経路探索にかかった時間: {stopWatch.Elapsed} ms");
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

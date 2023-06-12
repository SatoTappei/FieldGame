using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// A*を用いた経路探索を行うクラス
/// 単体でも使えるようにものびを継承している
/// </summary>
public class AStarSystem : MonoBehaviour, IPathfindingSystem
{
    [SerializeField] Transform _player;
    [SerializeField] AStarGrid _aStarGrid;

    AStarTask _aStarTask = new();
    Stack<Vector3> _path = new();

    void Start()
    {
        _aStarGrid.InitOnStart();

        // 毎フレームプレイヤーを中心にグリッドを生成する
        this.UpdateAsObservable().Subscribe(_ => _aStarGrid.Create(_player));
        // 破棄されるタイミングでJobSystem用に確保してあるNativeArrayを開放する
        this.OnDestroyAsObservable().Subscribe(_ => _aStarGrid.Dispose());
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

        AStarNode toNode = _aStarGrid.GetNode(_player.position);
        AStarNode fromNode = _aStarGrid.GetNode(pos);

        if (toNode == null || fromNode == null) return null;

        _path = _aStarTask.Execute(toNode, fromNode, _aStarGrid.Grid);

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
            foreach (Vector3 pos in _path)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }

        _aStarGrid.Visualize();
    }
}

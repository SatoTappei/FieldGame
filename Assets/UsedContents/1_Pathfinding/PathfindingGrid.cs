using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

/// <summary>
/// グリッドに敷き詰められるノードのクラス
/// </summary>
public class PathfindingNode
{
    public PathfindingNode(Vector3 pos, bool isPassable)
    {
        IsPassable = isPassable;
        Pos = pos;
    }

    public bool IsPassable { get; set; }
    public Vector3 Pos { get; }
}

/// <summary>
/// 経路探索で使用するグリッドのクラス
/// </summary>
[System.Serializable]
public class PathfindingGrid
{
    /// <summary>
    /// 理由がない限り1で固定。オブジェクト側が大きさを合わせれば良い。
    /// </summary>
    static readonly int NodeSize = 1;
    /// <summary>
    /// Gizmoで表示する際の倍率なのでインスペクターに表示させる必要なし
    /// </summary>
    static readonly float NodeGizmoViewMag = .5f;

    [Header("グリッドの設定")]
    [SerializeField] int _width = 100;
    [SerializeField] int _height = 100;
    [SerializeField] bool _visualizeOnGizmos = true;
    [Header("障害物を検知するRayの設定")]
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] float _obstacleRayRadius = 1.0f;
    [Tooltip("障害物を検知するRayを下向きに飛ばす基点となる高さ " + 
             "全ての障害物より高い位置からRayを飛ばさないときちんと判定されない")]
    [SerializeField] float _obstacleRayOriginY = 10.0f;
    [Header("Rayを飛ばす際のバッチ数の目安")]
    [SerializeField] int _commandsPerJob = 20;

    PathfindingNode[,] _grid;

    /// <summary>
    /// 外部からこのメソッドを呼ぶことで経路探索のためのグリッドを生成する
    /// 引数で渡したTransformを中心に生成される
    /// </summary>
    public void Create(Transform transform)
    {
        if (_grid != null)
        {
            Debug.LogWarning("既にグリッドを生成済み");
            return;
        }

        _grid = new PathfindingNode[_height, _width];
        
        NativeArray<RaycastHit> hits = new(_height * _width, Allocator.TempJob);
        NativeArray<SpherecastCommand> commands = new(_height * _width, Allocator.TempJob);
        
        for (int i = 0; i < _height; i++)
        {
            for(int k = 0; k < _width; k++)
            {
                // ノードを作成
                Vector3 pos = transform.position;
                pos.z += (i - (_height / 2)) * NodeSize;
                pos.x += (k - (_width / 2)) * NodeSize;
                _grid[i, k] = new PathfindingNode(pos, true);

                // 障害物を検知するためのRayを飛ばすための設定
                pos.y += _obstacleRayOriginY;
                QueryParameters queryParams = new() { layerMask = _obstacleLayer };
                commands[_height * i + k] = new SpherecastCommand()
                {
                    origin = pos,
                    direction = Vector3.down,
                    distance = _obstacleRayOriginY,
                    radius = _obstacleRayRadius,
                    queryParameters = queryParams,
                };
            }
        }

        
        JobHandle handle = SpherecastCommand.ScheduleBatch(commands, hits, _commandsPerJob, default(JobHandle));
        handle.Complete();

        // ジョブの結果を反映してそのノードが通行可能か判定する
        for (int i = 0; i < hits.Length; i++)
        {
            int iz = i / _width;
            int ix = i % _height;

            _grid[iz, ix].IsPassable = hits[i].collider == null;
        }

        hits.Dispose();
        commands.Dispose();
    }

    public Vector3 GetRandomPos()
    {
        int rz = Random.Range(0, _height);
        int rx = Random.Range(0, _width);

        // TODO: 通行可能なマスなのか判定する必要がある

        return _grid[rz, rx].Pos;
    }

    /// <summary>
    /// ギズモ上にグリッドを表示する
    /// 更新した場合も即座に反映されるようになっている
    /// </summary>
    public void Visualize()
    {
        if (_visualizeOnGizmos && _grid != null)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int k = 0; k < _width; k++)
                {
                    Gizmos.color = _grid[i, k].IsPassable ? Color.green : Color.red;
                    Gizmos.DrawCube(_grid[i, k].Pos, Vector3.one * NodeSize * NodeGizmoViewMag);
                }
            }
        }
    }
}

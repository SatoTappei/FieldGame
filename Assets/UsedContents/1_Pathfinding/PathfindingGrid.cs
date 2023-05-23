using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

/// <summary>
/// グリッドに敷き詰められるノードのクラス
/// </summary>
public class PathfindingNode
{
    public PathfindingNode(Vector3 pos, int z, int x, bool isPassable)
    {
        Pos = pos;
        Z = z;
        X = x;
        IsPassable = isPassable;
    }

    public PathfindingNode Parent { get; set; }
    public Vector3 Pos { get; set; }
    public int Z { get; private set; }
    public int X { get; private set; }
    public int ActualCost { get; set; }
    public int EstimateCost { get; set; }
    public bool IsPassable { get; set; }
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

    public PathfindingNode[,] Grid { get; private set; }

    /// <summary>
    /// ギズモに表示させるための座標
    /// 最後にGetNode()で返したノードの座標が入る
    /// </summary>
    Vector3? _gizmosHighlightNodePos;

    /// <summary>
    /// グリッドを何回も生成しなおすことを考慮して別途初期化用のメソッドを使用する
    /// </summary>
    public void InitOnStart()
    {
        Grid = new PathfindingNode[_height, _width];
        for(int i = 0; i < _height; i++)
        {
            for(int k = 0; k < _width; k++)
            {
                Grid[i, k] = new PathfindingNode(Vector3.zero, i, k, true);
            }
        }
    }

    /// <summary>
    /// 外部からこのメソッドを呼ぶことで経路探索のためのグリッドを生成する
    /// 引数で渡したTransformを中心に生成される
    /// </summary>
    public void Create(Transform transform)
    {
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
                Grid[i, k].Pos = pos;
                Grid[i, k].IsPassable = true;

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

            Grid[iz, ix].IsPassable = hits[i].collider == null;
        }

        hits.Dispose();
        commands.Dispose();
    }

    /// <summary>
    /// 座標に対応したノードを返す
    /// </summary>
    public PathfindingNode GetNode(Vector3 pos)
    {
        float forwardZ = Grid[0, 0].Pos.z;
        float backZ = Grid[_height - 1, _width - 1].Pos.z;
        float leftX = Grid[0, 0].Pos.x;
        float rightX = Grid[_height - 1, _width - 1].Pos.x;

        if (forwardZ <= pos.z && pos.z <= backZ && leftX <= pos.x && pos.x <= rightX)
        {
            // グリッドの1辺の長さ
            float lengthZ = backZ - forwardZ;
            float lengthX = rightX - leftX;
            // グリッドの端から座標までの長さ
            float fromPosZ = pos.z - forwardZ;
            float fromPosX = pos.x - leftX;
            // グリッドの端から何％の位置か
            float percentZ = Mathf.Abs(fromPosZ / lengthZ);
            float percentX = Mathf.Abs(fromPosX / lengthX);
            // 添え字に対応させる
            int indexZ = Mathf.RoundToInt((_height - 1) * percentZ);
            int indexX = Mathf.RoundToInt((_width - 1) * percentX);

            _gizmosHighlightNodePos = Grid[indexZ, indexX].Pos;
            return Grid[indexZ, indexX];
        }

        _gizmosHighlightNodePos = null;
        return null;
    }

    /// <summary>
    /// ギズモ上にグリッドを表示する
    /// 更新した場合も即座に反映されるようになっている
    /// </summary>
    public void Visualize()
    {
        if (_visualizeOnGizmos && Grid != null)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int k = 0; k < _width; k++)
                {
                    Gizmos.color = Grid[i, k].IsPassable ? Color.green : Color.red;
                    Gizmos.DrawCube(Grid[i, k].Pos, Vector3.one * NodeSize * NodeGizmoViewMag);
                }
            }

            if (_gizmosHighlightNodePos != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube((Vector3)_gizmosHighlightNodePos, Vector3.one * NodeSize * NodeGizmoViewMag);
            }
        }
    }
}

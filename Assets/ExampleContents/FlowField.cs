using UnityEngine;
using System.Collections.Generic;
using System;

namespace FlowField
{
    /// <summary>
    /// ゴールベースの経路探索アルゴリズム
    /// 大量のエージェントが1箇所に集合するために便利なアルゴリズム
    /// 大量のエージェントが移動するRTSや、レースゲームの最適なコースを走るために使用される
    /// 各地形にはコストがあるので、まずはコスト用のグリッドを生成する
    /// コスト用のグリッドを元に積分グリッドを生成する <- これはA*でいうノードのコスト
    /// </summary>
    public class FlowField
    {
        float _cellDiameter;

        public FlowField(float cellRadius, Vector2Int gridSize)
        {
            _cellRadius = cellRadius;
            _cellDiameter = cellRadius * 2;
            _gridSize = gridSize;
        }

        public Cell[,] _grid { get; private set; }
        public Vector2Int _gridSize { get; private set; }
        public float _cellRadius { get; private set; }
        public Cell _destinationCell;

        public void CreateGrid()
        {
            _grid = new Cell[_gridSize.x, _gridSize.y];

            for(int x = 0; x < _gridSize.x; x++)
            {
                for(int y = 0; y < _gridSize.y; y++)
                {
                    // セルの直径 * 座標 + セルの半径
                    Vector3 worldPos = new Vector3(_cellDiameter * x + _cellRadius, 0,
                        _cellDiameter * y + _cellRadius);
                    _grid[x, y] = new Cell(worldPos, new Vector2Int(x, y));
                }
            }
        }

        public void CreateCostField()
        {
            Vector3 cellHalfExtents = Vector3.one * _cellRadius;
            int terrainMask = LayerMask.GetMask("Impassible", "RoughTerrain");
            foreach(Cell currentCell in _grid)
            {
                // Rayを飛ばして壁や荒地だった場合はコストを増加させる
                Collider[] obstacles = Physics.OverlapBox(currentCell._worldPos, cellHalfExtents,
                    Quaternion.identity, terrainMask);
                // 複数のRayがグリッドにヒットした場合にコストの増加が重複して起こらない
                // ようにするためのフラグ(必要に応じて)
                bool hasIncreaseCost = false;
                foreach(Collider col in obstacles)
                {
                    if (col.gameObject.layer == 9)
                    {
                        currentCell.IncreaseCost(255);
                        continue;
                    }
                    else if(!hasIncreaseCost && col.gameObject.layer == 10)
                    {
                        currentCell.IncreaseCost(3);
                        hasIncreaseCost = true;
                    }
                }
            }
        }

        /// <summary>
        /// 統合フィールドを作成する
        /// A*っぽい処理をする？
        /// </summary>
        public void CreateintegrationField(Cell destinationCell)
        {
            _destinationCell = destinationCell;
            _destinationCell._cost = 0;
            _destinationCell._bestCost = 0;

            Queue<Cell> cellsToCheck = new();
            cellsToCheck.Enqueue(destinationCell);

            while (cellsToCheck.Count > 0)
            {
                Cell currentCell = cellsToCheck.Dequeue();
                List<Cell> currentNeighbours = GetNeighborCells(currentCell._gridIndex, 
                    GridDirection.CardinalDirections);
                foreach(Cell currentNeighbor in currentNeighbours)
                {
                    // コストが最大値ということは隣は壁
                    if (currentNeighbor._cost == byte.MaxValue) continue;
                    // ???: 周囲のCellのコストと現在のCellの最適コストが周囲のCellの最適コスト以下なら
                    if (currentNeighbor._cost + currentCell._bestCost < currentNeighbor._bestCost)
                    {
                        // 周囲のCellの最適コストの計算
                        // 周囲のCellのコスト + 現在のCellの最適コスト
                        currentNeighbor._bestCost = (ushort)(currentNeighbor._cost + currentCell._bestCost);
                        cellsToCheck.Enqueue(currentNeighbor);
                    }
                }
            }
        }

        public void CreateFlowField()
        {
            // グリッド全体
            foreach(Cell currentCell in _grid)
            {
                // このセルの周囲のCell
                List<Cell> currentNeighbors = GetNeighborCells(currentCell._gridIndex, GridDirection.AllDirections);
                int bestCost = currentCell._bestCost;

                // 周囲のCell全部に対して
                foreach(Cell currentNeighbor in currentNeighbors)
                {
                    // このセルの最適コストより最適コストが低ければ
                    if (currentNeighbor._bestCost < bestCost)
                    {
                        bestCost = currentNeighbor._bestCost;
                        // ターゲットへの方向ベクトルを求める感じで方向を求める
                        currentCell._bestDirection = GridDirection.GetDirectionFromV2I(
                            currentNeighbor._gridIndex - currentCell._gridIndex);
                    }
                }
            }
        }

        /// <summary>
        /// 指定したノードの周囲のノードを取得
        /// </summary>
        List<Cell> GetNeighborCells(Vector2Int nodeIndex, List<GridDirection> directions)
        {
            List<Cell> neighborCells = new();
            foreach(Vector2Int currentDirection in directions)
            {
                Cell newNeighbor = GetCellAtRelativePos(nodeIndex, currentDirection);
                if (newNeighbor != null)
                {
                    neighborCells.Add(newNeighbor);
                }
            }

            return neighborCells;
        }

        /// <summary>
        /// 実際にノードの周囲を調べる処理
        /// </summary>
        Cell GetCellAtRelativePos (Vector2Int originPos, Vector2Int relativePos)
        {
            Vector2Int finalPos = originPos + relativePos;
            if (finalPos.x < 0 || finalPos.x >= _gridSize.x ||
                finalPos.y < 0 || finalPos.y >= _gridSize.y)
            {
                return null;
            }
            else
            {
                return _grid[finalPos.x, finalPos.y];
            }
        }

        /// <summary>
        /// ★クリックした位置をグリッドに対応させる
        /// </summary>
        public Cell GetCellFromWorldPos(Vector3 worldPos)
        {
            float percentX = worldPos.x / (_gridSize.x * _cellDiameter);
            float percentY = worldPos.z / (_gridSize.y * _cellDiameter);

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.Clamp(Mathf.FloorToInt(_gridSize.x * percentX), 0, _gridSize.x - 1);
            int y = Mathf.Clamp(Mathf.FloorToInt(_gridSize.y * percentY), 0, _gridSize.y - 1);
            return _grid[x, y];
        }
    }
}

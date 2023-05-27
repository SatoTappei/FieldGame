using UnityEngine;
using System.Collections.Generic;
using System;

namespace FlowField
{
    /// <summary>
    /// �S�[���x�[�X�̌o�H�T���A���S���Y��
    /// ��ʂ̃G�[�W�F���g��1�ӏ��ɏW�����邽�߂ɕ֗��ȃA���S���Y��
    /// ��ʂ̃G�[�W�F���g���ړ�����RTS��A���[�X�Q�[���̍œK�ȃR�[�X�𑖂邽�߂Ɏg�p�����
    /// �e�n�`�ɂ̓R�X�g������̂ŁA�܂��̓R�X�g�p�̃O���b�h�𐶐�����
    /// �R�X�g�p�̃O���b�h�����ɐϕ��O���b�h�𐶐����� <- �����A*�ł����m�[�h�̃R�X�g
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
                    // �Z���̒��a * ���W + �Z���̔��a
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
                // Ray���΂��ĕǂ�r�n�������ꍇ�̓R�X�g�𑝉�������
                Collider[] obstacles = Physics.OverlapBox(currentCell._worldPos, cellHalfExtents,
                    Quaternion.identity, terrainMask);
                // ������Ray���O���b�h�Ƀq�b�g�����ꍇ�ɃR�X�g�̑������d�����ċN����Ȃ�
                // �悤�ɂ��邽�߂̃t���O(�K�v�ɉ�����)
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
        /// �����t�B�[���h���쐬����
        /// A*���ۂ�����������H
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
                    // �R�X�g���ő�l�Ƃ������Ƃׂ͕͗�
                    if (currentNeighbor._cost == byte.MaxValue) continue;
                    // ???: ���͂�Cell�̃R�X�g�ƌ��݂�Cell�̍œK�R�X�g�����͂�Cell�̍œK�R�X�g�ȉ��Ȃ�
                    if (currentNeighbor._cost + currentCell._bestCost < currentNeighbor._bestCost)
                    {
                        // ���͂�Cell�̍œK�R�X�g�̌v�Z
                        // ���͂�Cell�̃R�X�g + ���݂�Cell�̍œK�R�X�g
                        currentNeighbor._bestCost = (ushort)(currentNeighbor._cost + currentCell._bestCost);
                        cellsToCheck.Enqueue(currentNeighbor);
                    }
                }
            }
        }

        public void CreateFlowField()
        {
            // �O���b�h�S��
            foreach(Cell currentCell in _grid)
            {
                // ���̃Z���̎��͂�Cell
                List<Cell> currentNeighbors = GetNeighborCells(currentCell._gridIndex, GridDirection.AllDirections);
                int bestCost = currentCell._bestCost;

                // ���͂�Cell�S���ɑ΂���
                foreach(Cell currentNeighbor in currentNeighbors)
                {
                    // ���̃Z���̍œK�R�X�g���œK�R�X�g���Ⴏ���
                    if (currentNeighbor._bestCost < bestCost)
                    {
                        bestCost = currentNeighbor._bestCost;
                        // �^�[�Q�b�g�ւ̕����x�N�g�������߂銴���ŕ��������߂�
                        currentCell._bestDirection = GridDirection.GetDirectionFromV2I(
                            currentNeighbor._gridIndex - currentCell._gridIndex);
                    }
                }
            }
        }

        /// <summary>
        /// �w�肵���m�[�h�̎��͂̃m�[�h���擾
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
        /// ���ۂɃm�[�h�̎��͂𒲂ׂ鏈��
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
        /// ���N���b�N�����ʒu���O���b�h�ɑΉ�������
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

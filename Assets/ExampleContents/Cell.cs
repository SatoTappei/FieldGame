using UnityEngine;

namespace FlowField
{
    public class Cell
    {
        public Vector3 _worldPos;
        public Vector2Int _gridIndex;
        public byte _cost;
        public ushort _bestCost;
        public GridDirection _bestDirection;

        public Cell(Vector3 worldPos, Vector2Int gridIndex)
        {
            _worldPos = worldPos;
            _gridIndex = gridIndex;
            _cost = 1;
            _bestCost = ushort.MaxValue;
            _bestDirection = GridDirection.None;
        }

        /// <summary>
        /// 1~255ÇÃä‘Ç≈ÉRÉXÉgÇÃëùâ¡
        /// </summary>
        public void IncreaseCost(int amnt)
        {
            if (_cost == byte.MaxValue) return;
            
            if (amnt + _cost >= 255)
            {
                _cost = byte.MaxValue;
            }
            else
            {
                _cost += (byte)amnt;
            }
        }
    }
}
using UnityEngine;

namespace FlowField
{
    public class GridController : MonoBehaviour
    {
        enum GizmoDrawType
        {
            Cost,
            Integration,
        }

        public Vector2Int _gridSize;
        public float _cellRadius = 0.5f;
        public FlowField _currentFlowField;

        [SerializeField] GridDebug _gridDebug;
        [SerializeField] GizmoDrawType _gizmoDrawType;

        void Start()
        {
            InitializeFlowField();
        }

        void Update()
        {

        }

        public void InitializeFlowField()
        {
            _currentFlowField = new FlowField(_cellRadius, _gridSize);
            _currentFlowField.CreateGrid();

            _currentFlowField.CreateCostField();

            // マウスの位置に目的地を設定する処理
            //Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            //Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            //Cell destinationCell = _currentFlowField.GetCellFromWorldPos(worldMousePos);
            Cell destinationCell = _currentFlowField._grid[Random.Range(0, _gridSize.x - 1), Random.Range(0, _gridSize.y - 1)];
            _currentFlowField.CreateintegrationField(destinationCell);

            _currentFlowField.CreateFlowField();
            _gridDebug.SetFlowField(_currentFlowField);
            _gridDebug.DrawFlowField();
        }

        void OnDrawGizmos()
        {
            DrawGrid(_gridSize, Color.green, _cellRadius);
            DrawCost();
        }

        void DrawGrid(Vector2Int drawGridSize, Color drawColor, float drawCellRadius)
        {
            Gizmos.color = drawColor;
            for(int x = 0; x < drawGridSize.x; x++)
            {
                for(int y = 0; y < drawGridSize.y; y++)
                {
                    Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, 0,
                        drawCellRadius * 2 * y + drawCellRadius);
                    Vector3 size = Vector3.one * drawCellRadius * 2;
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }

        void DrawCost()
        {
            if (_currentFlowField == null) return;

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 6;

            foreach(Cell currentCell in _currentFlowField._grid)
            {
                if(_gizmoDrawType == GizmoDrawType.Cost)
                {
                    UnityEditor.Handles.Label(currentCell._worldPos, currentCell._cost.ToString(), style);
                }
                else
                {
                    UnityEditor.Handles.Label(currentCell._worldPos, currentCell._bestCost.ToString(), style);
                }
            }
        }
    }
}

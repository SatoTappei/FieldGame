using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PathfindingSystem : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;
    [SerializeField] PathfindingModule pathfindingModule;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //_pathfindingGrid.Create(transform);

        _pathfindingGrid.InitOnStart();

        // 毎フレームプレイヤーを中心にグリッドを生成する
        this.UpdateAsObservable().Subscribe(_ => _pathfindingGrid.Create(player.transform));
    }

    public void OnDrawGizmos()
    {
        _pathfindingGrid.Visualize();
    }
}

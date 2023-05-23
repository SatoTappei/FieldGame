using UnityEngine;

/// <summary>
/// 経路探索をするキャラクターのテスト
/// </summary>
public class ExamplePathfindingActor : MonoBehaviour
{
    void Start()
    {
    }

    public void Execute()
    {
        // TODO:実際はシングルトンとかにしておく
        PathfindingSystem system = FindObjectOfType<PathfindingSystem>();
        system.GetPath(transform.position);
    }
}

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索の機能を使うクラス
/// 各敵が経路探索をする際に使用するそれぞれのアルゴリズムを管理する
/// </summary>
[RequireComponent(typeof(AStarSystem))]
public class PathfindingManager : MonoBehaviour
{
    public static PathfindingManager Instance { get; private set; }

    [SerializeField] Transform _player;

    LinerPathSystem _linerPathSystem;
    AStarSystem _aStarSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _aStarSystem = GetComponent<AStarSystem>();
            _linerPathSystem = new(_player);
        }
        else
        {
            Destroy(this);
        }
    }

    public Stack<Vector3> GetPath(Vector3 pos)
    {
        return _aStarSystem.GetPath(pos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingModule : MonoBehaviour
{
    [SerializeField] PathfindingGrid _pathfindingGrid;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _pathfindingGrid.Create(transform);
        player.transform.position = _pathfindingGrid.GetRandomPos();
    }

    public void OnDrawGizmos()
    {
        _pathfindingGrid.Visualize();
    }
}

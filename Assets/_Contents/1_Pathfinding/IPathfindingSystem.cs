using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 経路探索を行うクラスだと保証するインターフェース
/// </summary>
public interface IPathfindingSystem
{
    public Stack<Vector3> GetPath(Vector3 pos);
}

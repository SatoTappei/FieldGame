using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �o�H�T�����s���N���X���ƕۏ؂���C���^�[�t�F�[�X
/// </summary>
public interface IPathfindingSystem
{
    public Stack<Vector3> GetPath(Vector3 pos);
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A*��p���ăO���b�h����o�H�����߂鏈���𔲂��o�����N���X
/// </summary>
public class PathfindingTask
{
    /// <summary>
    /// �o�H�����߂�̂Ɍv�Z������ő��
    /// </summary>
    static readonly int MaxPathDistance = 100;

    public Stack<Vector3> Execute(PathfindingNode to, PathfindingNode from, PathfindingNode[,] grid)
    {
        BinaryHeap<PathfindingNode> openSet = new(MaxPathDistance * 8);
        HashSet<PathfindingNode> closedSet = new();
        
        openSet.Add(grid[from.Z, from.X]);

        int count = 0;
        List<PathfindingNode> neighbourList = new(8);
        while (count++ < MaxPathDistance)
        {
            PathfindingNode current = openSet.Pop();
            closedSet.Add(current);

            // �ړI�n�ɓ��������ꍇ
            if (current.Z == to.Z && current.X == to.X)
            {
                return TraceParent(to, from);
            }

            // ����8��
            neighbourList.Clear();
            GetNeighbour(current.Z, current.X, neighbourList, grid);

            // ����8�̃m�[�h�̃R�X�g���v�Z
            foreach (PathfindingNode neighbour in neighbourList)
            {
                // close�̃��X�g�Ɋ܂܂�Ă�����Ȃ�
                if (closedSet.Contains(neighbour)) continue;

                // �ׂ̃m�[�h�܂ł̃��[�N���b�h���������R�X�g�Ƃ���
                int additionalCost = CalcDistance(neighbour.Z, neighbour.X, current.Z, current.X);
                int neighbourActualCost = current.ActualCost + additionalCost;

                // open�ȃ��X�g�Ɋ܂܂�Ă��Ȃ��������͎��R�X�g�����Ⴂ�ꍇ�̓m�[�h���X�V����
                bool unContains = !openSet.Contains(neighbour);
                if (neighbourActualCost < neighbour.ActualCost || unContains)
                {
                    neighbour.ActualCost = neighbourActualCost;
                    neighbour.EstimateCost = CalcDistance(neighbour.Z, neighbour.X, to.Z, to.X);
                    neighbour.Parent = current;

                    if (unContains) openSet.Add(neighbour);
                }
            }
        }

        Debug.LogWarning($"{from.Pos}����{to.Pos}�ւ̌o�H�T���Ɏ��s");
        return null;
    }

    Stack<Vector3> TraceParent(PathfindingNode to, PathfindingNode from)
    {
        PathfindingNode goalNode = to;
        Stack<Vector3> path = new();
        while (goalNode != from)
        {
            path.Push(goalNode.Pos);
            goalNode = goalNode.Parent;
        }

        return path;
    }

    void GetNeighbour(int z, int x, List<PathfindingNode> list, PathfindingNode[,] grid)
    {
        for(int i = -1; i <= 1; i++)
        {
            for(int k = -1; k <= 1; k++)
            {
                if (i == 0 && k == 0) continue;
                if (z + i < 0 || grid.GetLength(0) <= z + i ||
                    x + k < 0 || grid.GetLength(1) <= x + k) continue;
                if (!grid[z + i, x + k].IsPassable) continue;

                list.Add(grid[z + i, x + k]);
            }
        }
    }

    int CalcDistance(int toZ, int toX, int fromZ, int fromX)
    {
        int distZ = Mathf.Abs(toZ - fromZ);
        int distX = Mathf.Abs(toX - fromX);

        if (distZ > distX)
        {
            return distX * 14 + (distZ - distX) * 10;
        }
        else
        {
            return distZ * 14 + (distX - distZ) * 10;
        }
    }
}

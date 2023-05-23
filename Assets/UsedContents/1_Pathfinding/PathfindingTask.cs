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

    public Queue<Vector3> Execute(PathfindingNode to, PathfindingNode from, PathfindingNode[,] grid)
    {
        List<PathfindingNode> openList = new();
        List<PathfindingNode> closedList = new();

        openList.Add(grid[from.Z, from.X]);

        int count = 0;
        while(count++ < MaxPathDistance)
        {
            // �R�X�g����ԒႢ�m�[�h��I��
            PathfindingNode current = openList.OrderBy(node => node.ActualCost + node.EstimateCost)
                .ThenBy(node => node.ActualCost).FirstOrDefault();

            // ���݂̃m�[�h��open����close��
            openList.Remove(current);
            closedList.Add(current);

            // �ړI�n�ɓ�������
            if (current.Z == to.Z && current.X == to.X)
            {
                PathfindingNode goalNode = current;
                Queue<Vector3> path = new();
                while (goalNode.Parent != from)
                {
                    path.Enqueue(goalNode.Pos);
                    goalNode = goalNode.Parent;
                }

                return path;
            }

            // ����8��
            List<PathfindingNode> neighbourList = new(8);
            GetNeighbour(current.Z, current.X, neighbourList, grid);

            // ����8�̃m�[�h�̃R�X�g���v�Z
            foreach (PathfindingNode neighbour in neighbourList)
            {
                // close�̃��X�g�Ɋ܂܂�Ă�����Ȃ�
                if (closedList.Contains(neighbour)) continue;

                int additionalCost = CalcDistance(neighbour.Z, neighbour.X, current.Z, current.X);
                int neighbourActualCost = current.ActualCost + additionalCost;

                bool already = openList.Contains(neighbour);
                if (neighbourActualCost < neighbour.ActualCost || !already)
                {
                    neighbour.ActualCost = neighbourActualCost;
                    neighbour.EstimateCost = CalcDistance(neighbour.Z, neighbour.X, to.Z, to.X);
                    neighbour.Parent = current;

                    if (!already)
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }

        return null;
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

                if (grid[z + i, x + k].IsPassable)
                {
                    list.Add(grid[z + i, x + k]);
                }
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

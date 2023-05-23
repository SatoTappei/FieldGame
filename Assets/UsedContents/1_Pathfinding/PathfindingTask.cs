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

        PathfindingNode current = grid[from.Z, from.X];
        closedList.Add(current);

        int count = 0;
        while(count++ < MaxPathDistance)
        {
            // ����8��
            List<PathfindingNode> neighbourList = new(8);
            GetNeighbour(current.Z, current.X, neighbourList, grid);

            // ����8�̃m�[�h�̃R�X�g���v�Z
            foreach (PathfindingNode node in neighbourList)
            {
                node.Parent = current;

                if (node.Z == to.Z && node.X == to.X)
                {
                    PathfindingNode goalNode = node;
                    Queue<Vector3> path = new();
                    while (goalNode.Parent != null)
                    {
                        path.Enqueue(goalNode.Pos);
                        goalNode = node.Parent;
                    }

                    return path;
                }

                node.ActualCost += current.ActualCost + 1;
                node.EstimateCost = CalcEstimateCost(to.Z, from.Z, to.X, from.X);
            }

            // �J�����m�[�h�̃��X�g�ɒǉ�
            openList.AddRange(neighbourList);

            // �����m�[�h�̃��X�g�ɒǉ�
            closedList.Add(current);

            // �R�X�g����ԒႢ�m�[�h�����̃m�[�h�ɐݒ�
            current = openList.OrderBy(node => node.ActualCost + node.EstimateCost).ThenBy(node => node.ActualCost).FirstOrDefault();
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

                if (grid[z + i, x + k].IsPassable)
                {
                    list.Add(grid[z + i, x + k]);
                }
            }
        }
    }

    int CalcEstimateCost(int toZ, int fromZ, int toX, int fromX)
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

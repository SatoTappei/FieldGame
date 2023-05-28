using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System;

/// <summary>
/// �O���b�h�ɕ~���l�߂���m�[�h�̃N���X
/// </summary>
public class AStarNode : IBinaryHeapCollectable<AStarNode>
{
    public AStarNode(Vector3 pos, int z, int x, bool isPassable)
    {
        Pos = pos;
        Z = z;
        X = x;
        IsPassable = isPassable;
    }

    public AStarNode Parent { get; set; }
    public Vector3 Pos { get; set; }
    public int Z { get; private set; }
    public int X { get; private set; }
    public int ActualCost { get; set; }
    public int EstimateCost { get; set; }
    public int TotalCost => ActualCost + EstimateCost;
    public bool IsPassable { get; set; }

    public int BinaryHeapIndex { get; set; }
    /// <summary>
    /// �R�X�g���傫�����ǂ����̌��ʂ�Ԃ��Ă���̂�
    /// �ŏ��̃R�X�g�Ń\�[�g�������ꍇ�͌��ʂ𔽓]����K�v������
    /// </summary>
    public int CompareTo(AStarNode other)
    {
        int result = TotalCost.CompareTo(other.TotalCost);
        if (result == 0)
        {
            result = EstimateCost.CompareTo(other.EstimateCost);
        }

        return result;
    }
}

/// <summary>
/// �o�H�T���Ŏg�p����O���b�h�̃N���X
/// </summary>
[System.Serializable]
public class AStarGrid : IDisposable
{
    /// <summary>
    /// ���R���Ȃ�����1�ŌŒ�B�I�u�W�F�N�g�����傫�������킹��Ηǂ�
    /// </summary>
    static readonly int NodeSize = 1;
    /// <summary>
    /// Gizmo�ŕ\������ۂ̔{���Ȃ̂ŃC���X�y�N�^�[�ɕ\��������K�v�Ȃ�
    /// </summary>
    static readonly float NodeGizmoViewMag = .5f;

    [Header("�O���b�h�̐ݒ�")]
    [SerializeField] int _width = 100;
    [SerializeField] int _height = 100;
    [SerializeField] bool _visualizeOnGizmos = true;
    [Header("��Q�������m����Ray�̐ݒ�")]
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] float _obstacleRayRadius = 1.0f;
    [Tooltip("��Q�������m����Ray���������ɔ�΂���_�ƂȂ鍂�� " + 
             "�S�Ă̏�Q����荂���ʒu����Ray���΂��Ȃ��Ƃ�����Ɣ��肳��Ȃ�")]
    [SerializeField] float _obstacleRayOriginY = 10.0f;
    [Header("Ray���΂��ۂ̃o�b�`���̖ڈ�")]
    [SerializeField] int _commandsPerJob = 20;

    public AStarNode[,] Grid { get; private set; }

    /// <summary>
    /// �M�Y���ɕ\�������邽�߂̍��W
    /// �Ō��GetNode()�ŕԂ����m�[�h�̍��W������
    /// </summary>
    Vector3? _gizmosHighlightNodePos;

    NativeArray<RaycastHit> _hits;
    NativeArray<SpherecastCommand> _commands;

    /// <summary>
    /// �O���b�h��������������Ȃ������Ƃ��l�����ĕʓr�������p�̃��\�b�h���g�p����
    /// </summary>
    public void InitOnStart()
    {
        Grid = new AStarNode[_height, _width];
        for(int i = 0; i < _height; i++)
        {
            for(int k = 0; k < _width; k++)
            {
                Grid[i, k] = new AStarNode(Vector3.zero, i, k, true);
            }
        }

        _hits = new(_height * _width, Allocator.Persistent);
        _commands = new(_height * _width, Allocator.Persistent);
    }

    /// <summary>
    /// �O�����炱�̃��\�b�h���ĂԂ��ƂŌo�H�T���̂��߂̃O���b�h�𐶐�����
    /// �����œn����Transform�𒆐S�ɐ��������
    /// </summary>
    public void Create(Transform transform)
    {
        //NativeArray<RaycastHit> hits = new(_height * _width, Allocator.TempJob);
        //NativeArray<SpherecastCommand> commands = new(_height * _width, Allocator.TempJob);
        
        for (int i = 0; i < _height; i++)
        {
            for(int k = 0; k < _width; k++)
            {
                // �m�[�h���쐬
                Vector3 pos = transform.position;
                pos.z += (i - (_height / 2)) * NodeSize;
                pos.x += (k - (_width / 2)) * NodeSize;
                Grid[i, k].Pos = pos;
                Grid[i, k].IsPassable = true;

                // ��Q�������m���邽�߂�Ray���΂����߂̐ݒ�
                pos.y += _obstacleRayOriginY;
                QueryParameters queryParams = new() { layerMask = _obstacleLayer };
                _commands[_height * i + k] = new SpherecastCommand()
                {
                    origin = pos,
                    direction = Vector3.down,
                    distance = _obstacleRayOriginY,
                    radius = _obstacleRayRadius,
                    queryParameters = queryParams,
                };
            }
        }

        JobHandle handle = SpherecastCommand.ScheduleBatch(_commands, _hits, _commandsPerJob, default(JobHandle));
        handle.Complete();

        // �W���u�̌��ʂ𔽉f���Ă��̃m�[�h���ʍs�\�����肷��
        for (int i = 0; i < _hits.Length; i++)
        {
            int iz = i / _width;
            int ix = i % _height;

            Grid[iz, ix].IsPassable = _hits[i].collider == null;
        }

        //_hits.Dispose();
        //_commands.Dispose();
    }

    /// <summary>
    /// ���W�ɑΉ������m�[�h��Ԃ�
    /// </summary>
    public AStarNode GetNode(Vector3 pos)
    {
        float forwardZ = Grid[0, 0].Pos.z;
        float backZ = Grid[_height - 1, _width - 1].Pos.z;
        float leftX = Grid[0, 0].Pos.x;
        float rightX = Grid[_height - 1, _width - 1].Pos.x;

        if (forwardZ <= pos.z && pos.z <= backZ && leftX <= pos.x && pos.x <= rightX)
        {
            // �O���b�h��1�ӂ̒���
            float lengthZ = backZ - forwardZ;
            float lengthX = rightX - leftX;
            // �O���b�h�̒[������W�܂ł̒���
            float fromPosZ = pos.z - forwardZ;
            float fromPosX = pos.x - leftX;
            // �O���b�h�̒[���牽���̈ʒu��
            float percentZ = Mathf.Abs(fromPosZ / lengthZ);
            float percentX = Mathf.Abs(fromPosX / lengthX);
            // �Y�����ɑΉ�������
            int indexZ = Mathf.RoundToInt((_height - 1) * percentZ);
            int indexX = Mathf.RoundToInt((_width - 1) * percentX);

            _gizmosHighlightNodePos = Grid[indexZ, indexX].Pos;
            return Grid[indexZ, indexX];
        }

        Debug.Log("�m�[�h���O���b�h�͈̔͊O�Ȃ̂�null��Ԃ�: " + pos);
        _gizmosHighlightNodePos = null;
        return null;
    }

    /// <summary>
    /// �M�Y����ɃO���b�h��\������
    /// �X�V�����ꍇ�������ɔ��f�����悤�ɂȂ��Ă���
    /// </summary>
    public void Visualize()
    {
        if (_visualizeOnGizmos && Grid != null)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int k = 0; k < _width; k++)
                {
                    Gizmos.color = Grid[i, k].IsPassable ? Color.green : Color.red;
                    Gizmos.DrawCube(Grid[i, k].Pos, Vector3.one * NodeSize * NodeGizmoViewMag);
                }
            }

            if (_gizmosHighlightNodePos != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube((Vector3)_gizmosHighlightNodePos, Vector3.one * NodeSize * NodeGizmoViewMag);
            }
        }
    }

    public void Dispose()
    {
        _hits.Dispose();
        _commands.Dispose();
    }
}

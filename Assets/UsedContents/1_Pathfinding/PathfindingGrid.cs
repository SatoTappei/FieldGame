using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

/// <summary>
/// �O���b�h�ɕ~���l�߂���m�[�h�̃N���X
/// </summary>
public class PathfindingNode
{
    public PathfindingNode(Vector3 pos, bool isPassable)
    {
        IsPassable = isPassable;
        Pos = pos;
    }

    public bool IsPassable { get; set; }
    public Vector3 Pos { get; }
}

/// <summary>
/// �o�H�T���Ŏg�p����O���b�h�̃N���X
/// </summary>
[System.Serializable]
public class PathfindingGrid
{
    /// <summary>
    /// ���R���Ȃ�����1�ŌŒ�B�I�u�W�F�N�g�����傫�������킹��Ηǂ��B
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

    PathfindingNode[,] _grid;

    /// <summary>
    /// �O�����炱�̃��\�b�h���ĂԂ��ƂŌo�H�T���̂��߂̃O���b�h�𐶐�����
    /// �����œn����Transform�𒆐S�ɐ��������
    /// </summary>
    public void Create(Transform transform)
    {
        if (_grid != null)
        {
            Debug.LogWarning("���ɃO���b�h�𐶐��ς�");
            return;
        }

        _grid = new PathfindingNode[_height, _width];
        
        NativeArray<RaycastHit> hits = new(_height * _width, Allocator.TempJob);
        NativeArray<SpherecastCommand> commands = new(_height * _width, Allocator.TempJob);
        
        for (int i = 0; i < _height; i++)
        {
            for(int k = 0; k < _width; k++)
            {
                // �m�[�h���쐬
                Vector3 pos = transform.position;
                pos.z += (i - (_height / 2)) * NodeSize;
                pos.x += (k - (_width / 2)) * NodeSize;
                _grid[i, k] = new PathfindingNode(pos, true);

                // ��Q�������m���邽�߂�Ray���΂����߂̐ݒ�
                pos.y += _obstacleRayOriginY;
                QueryParameters queryParams = new() { layerMask = _obstacleLayer };
                commands[_height * i + k] = new SpherecastCommand()
                {
                    origin = pos,
                    direction = Vector3.down,
                    distance = _obstacleRayOriginY,
                    radius = _obstacleRayRadius,
                    queryParameters = queryParams,
                };
            }
        }

        
        JobHandle handle = SpherecastCommand.ScheduleBatch(commands, hits, _commandsPerJob, default(JobHandle));
        handle.Complete();

        // �W���u�̌��ʂ𔽉f���Ă��̃m�[�h���ʍs�\�����肷��
        for (int i = 0; i < hits.Length; i++)
        {
            int iz = i / _width;
            int ix = i % _height;

            _grid[iz, ix].IsPassable = hits[i].collider == null;
        }

        hits.Dispose();
        commands.Dispose();
    }

    public Vector3 GetRandomPos()
    {
        int rz = Random.Range(0, _height);
        int rx = Random.Range(0, _width);

        // TODO: �ʍs�\�ȃ}�X�Ȃ̂����肷��K�v������

        return _grid[rz, rx].Pos;
    }

    /// <summary>
    /// �M�Y����ɃO���b�h��\������
    /// �X�V�����ꍇ�������ɔ��f�����悤�ɂȂ��Ă���
    /// </summary>
    public void Visualize()
    {
        if (_visualizeOnGizmos && _grid != null)
        {
            for (int i = 0; i < _height; i++)
            {
                for (int k = 0; k < _width; k++)
                {
                    Gizmos.color = _grid[i, k].IsPassable ? Color.green : Color.red;
                    Gizmos.DrawCube(_grid[i, k].Pos, Vector3.one * NodeSize * NodeGizmoViewMag);
                }
            }
        }
    }
}

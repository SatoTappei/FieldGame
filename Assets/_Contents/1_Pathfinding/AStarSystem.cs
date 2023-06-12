using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

/// <summary>
/// A*��p�����o�H�T�����s���N���X
/// �P�̂ł��g����悤�ɂ��̂т��p�����Ă���
/// </summary>
public class AStarSystem : MonoBehaviour, IPathfindingSystem
{
    [SerializeField] Transform _player;
    [SerializeField] AStarGrid _aStarGrid;

    AStarTask _aStarTask = new();
    Stack<Vector3> _path = new();

    void Start()
    {
        _aStarGrid.InitOnStart();

        // ���t���[���v���C���[�𒆐S�ɃO���b�h�𐶐�����
        this.UpdateAsObservable().Subscribe(_ => _aStarGrid.Create(_player));
        // �j�������^�C�~���O��JobSystem�p�Ɋm�ۂ��Ă���NativeArray���J������
        this.OnDestroyAsObservable().Subscribe(_ => _aStarGrid.Dispose());
    }

    /// <summary>
    /// �v���C���[�ւ̌o�H��T���������Ƃ��ɊO������Ăяo�����\�b�h
    /// </summary>
    public Stack<Vector3> GetPath(Vector3 pos)
    {
#if UNITY_EDITOR
        System.Diagnostics.Stopwatch stopWatch = new();
        stopWatch.Start();
#endif

        AStarNode toNode = _aStarGrid.GetNode(_player.position);
        AStarNode fromNode = _aStarGrid.GetNode(pos);

        if (toNode == null || fromNode == null) return null;

        _path = _aStarTask.Execute(toNode, fromNode, _aStarGrid.Grid);

#if UNITY_EDITOR
        stopWatch.Stop();
        Debug.Log($"�o�H�T���ɂ�����������: {stopWatch.Elapsed} ms");
#endif

        return _path;
    }

    public void OnDrawGizmos()
    {
        if (_path != null)
        {
            foreach (Vector3 pos in _path)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(pos, Vector3.one);
            }
        }

        _aStarGrid.Visualize();
    }
}

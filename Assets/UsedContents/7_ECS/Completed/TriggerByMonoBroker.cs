using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���˂����e�̏��̍\����
/// ���b�Z�[�W���O�p�ł͂Ȃ��AECS���ɓn���������b�v���邽�߂Ɏg��
/// </summary>
public struct ShootData
{
    public ShootData(Vector3 pos, Vector3 dir)
    {
        Pos = pos;
        Dir = dir;
    }

    public Vector3 Pos { get; private set; }
    public Vector3 Dir { get; private set; }
}

/// <summary>
/// GameObject����ECS���̒�����ƂȂ�N���X
/// </summary>
public class TriggerByMonoBroker : MonoBehaviour
{
    public static TriggerByMonoBroker Instance { get; private set; }

    Queue<ShootData> _queue;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _queue = new();
        }
        else
        {
            Destroy(this);
        }
    }

    // �e�X�g�p:�{�^���Ɋ��蓖�Ă�
    public void TestFire()
    {
        AddShootData(Vector3.zero, Vector3.forward);
    }

    public void AddShootData(Vector3 pos, Vector3 dir)
    {
        _queue.Enqueue(new ShootData(pos, dir));
    }

    // TODO:1�t���[����1���o�����ƁA��ʂɃL���[�C���O���Ă������ꍇ�ɔ���ƈʒu���Y����
    public bool TryGetShootData(out ShootData data)
    {
        return _queue.TryDequeue(out data);
    }
}

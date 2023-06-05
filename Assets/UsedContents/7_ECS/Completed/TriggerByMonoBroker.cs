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

    // �e�X�g�p:���̂܂܂ł�����ɉe���͖���
    [SerializeField] Transform[] _muzzles;

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
    //          ���̂܂܂ł�����ɉe���͖���
    public void TestFire()
    {
        for(int i = 0; i < 100; i++)
        {
            int r = Random.Range(0, _muzzles.Length);
            Vector3 pos = _muzzles[r].position;

            float x = Random.Range(-0.25f, 0.25f);
            float y = Random.Range(-0.25f, 0.25f);
            Vector3 normalizedDir = new Vector3(x, y, 1.0f).normalized;

            AddShootData(pos, normalizedDir);
        }
    }

    /// <summary>
    /// ��ʂɏo�����߂̃f�o�b�O�p�̔���
    /// Player���͂��̃��\�b�h���ĂԂ���
    /// </summary>
    public void DebugAddShootData(Vector3 pos, Vector3 dir)
    {
        for (int i = 0; i < 100; i++)
        {
            float x = Random.Range(-0.25f, 0.25f);
            float y = Random.Range(-0.25f, 0.25f);
            float z = Random.Range(-0.25f, 0.25f);
            Vector3 normalizedDir = new Vector3(dir.x + x, dir.y + y, dir.z + z).normalized;

            AddShootData(pos, normalizedDir);
        }
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

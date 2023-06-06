using UnityEngine;

/// <summary>
/// �v���C���[�ƓG�̃_���[�W�̃f�[�^�̍\����
/// ���̍\���̂����b�Z�[�W���O�ł���肷�邱�ƂŃ_���[�W�̏������s��
/// </summary>
public struct DamageData
{
    public enum TargetTag
    {
        Player,
        Enemy,
    }

    public DamageData(Vector3 hitPos, float range, TargetTag target)
    {
        HitPos = hitPos;
        Range = range;
        Tag = target.ToString();
    }


    public Vector3 HitPos { get; }
    public float Range { get; }
    public string Tag { get; }
}

/// <summary>
/// InputSystem�̓��͂�؂�ւ���f�[�^�̍\����
/// ���̍\���̂����b�Z�[�W���O�ł���肷�邱�Ƃ�InputSystem�Ŏg�����͂̐؂�ւ����s��
/// </summary>
public struct InputTypeData
{
    public enum InputType
    {
        Player,
        UI,
    }

    public InputTypeData(InputType type)
    {
        Type = type;
    }

    public InputType Type { get; }
}

/// <summary>
/// �^�C�g������C���Q�[���ɑJ�ڂ����邽�߂̍\����
/// �Q�[���X�^�[�g�{�^�����������ۂɑ��M�����
/// �C���Q�[���J�n�����g���K�[�������ꍇ�͂��̃��b�Z�[�W����M����
/// </summary>
struct ToInGameTrigger
{
}

/// <summary>
/// �v���C���[�̃_���[�W���󂯂�O�ƌ�̗̑͂̍\����
/// �v���C���[���_���[�W���󂯂��ꍇ�Ƀ��b�Z�[�W���O�����
/// </summary>
public struct PlayerLifePointData
{
    public PlayerLifePointData(int befor, int after)
    {
        BeforValue = befor;
        AfterValue = after;
    }

    public int BeforValue { get; }
    public int AfterValue { get; }
}

/// <summary>
/// �\������䎌�̍\����
/// </summary>
public struct LineData
{
    public LineData(string name, string line)
    {
        Name = name;
        Line = line;
    }

    public string Name { get; }
    public string Line { get; }
}
using UnityEngine;

// 2�Ƃ����g�p

/// <summary>
/// Awake�̃^�C�~���O�ŏ��������鏈�������邱�Ƃ𖾎����邽�߂̃C���^�[�t�F�[�X
/// ���������N���X�������́A���̃N���X�����W���[���Ƃ��Ď���MonoBehavior��Awake()�ŌĂԑz��
/// </summary>
public interface IInitializeOnAwake
{
    public void InitOnAwake(GameObject gameObject = null);
}

/// <summary>
/// Start�̃^�C�~���O�ŏ��������鏈�������邱�Ƃ𖾎����邽�߂̃C���^�[�t�F�[�X
/// ���������N���X�������́A���̃N���X�����W���[���Ƃ��Ď���MonoBehavior��Start()�ŌĂԑz��
/// </summary>
public interface IInitializeOnStart
{
    public void InitOnStart(GameObject gameObject = null);
}
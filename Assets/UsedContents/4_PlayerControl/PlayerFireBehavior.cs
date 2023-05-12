using UnityEngine;

/// <summary>
/// �v���C���[�̍U���Ɋւ��鏈�����s���N���X
/// </summary>
[System.Serializable]
public class PlayerFireBehavior
{
    /// <summary>
    /// InputSystem�ɓo�^���čU���t���O��؂�ւ��邽�߂�SetGet�ǂ�������J����K�v������
    /// </summary>
    public bool IsFiring { get; set; }

    public void Update()
    {
        if (IsFiring)
        {
            Debug.Log("�U����");
            GameManager.Instance.AudioModule.PlaySE(AudioType.SE_Fire);
        }
    }
}

using UnityEngine;

/// <summary>
/// �v���C���[�̃��b�N�I���@�\�̏������s���N���X
/// </summary>
[System.Serializable]
public class PlayerFocusBehavior : IInputActionRegistrable
{
    public void RegisterInputAction(InputActionRegister register)
    {
        register.OnFocus += FocusForward;
    }

    void FocusForward()
    {
        // ���ʂ�SphereCollider���΂��ă��b�N�I������
        Debug.Log("���b�N�I��");
    }
}

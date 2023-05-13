using UnityEngine;

/// <summary>
/// プレイヤーのロックオン機能の処理を行うクラス
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
        // 正面にSphereColliderを飛ばしてロックオンする
        Debug.Log("ロックオン");
    }
}

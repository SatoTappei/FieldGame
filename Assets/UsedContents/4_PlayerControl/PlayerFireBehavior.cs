using UnityEngine;

/// <summary>
/// プレイヤーの攻撃に関する処理を行うクラス
/// </summary>
[System.Serializable]
public class PlayerFireBehavior
{
    /// <summary>
    /// InputSystemに登録して攻撃フラグを切り替えるためにSetGetどちらも公開する必要がある
    /// </summary>
    public bool IsFiring { get; set; }

    public void Update()
    {
        if (IsFiring)
        {
            Debug.Log("攻撃中");
            GameManager.Instance.AudioModule.PlaySE(AudioType.SE_Fire);
        }
    }
}

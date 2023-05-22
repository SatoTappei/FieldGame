using UnityEngine;

/// <summary>
/// 仮のダメージを与えるメッセージを送信するクラス
/// ボタンに割り当てて使用する
/// </summary>
public class TempDamageMessageSystem : MonoBehaviour
{
    public void Send()
    {
        DamageMessageSender.SendMessageToEnemy(Vector3.zero, 10000);
    }
}

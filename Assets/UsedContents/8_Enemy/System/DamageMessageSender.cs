using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーと敵のダメージのデータの構造体
/// この構造体をメッセージングでやり取りすることでダメージの処理を行う
/// </summary>
public struct DamageData
{
    public DamageData(Vector3 hitPos, float range, string targetTag)
    {
        HitPos = hitPos;
        Range = range;
        TargetTag = targetTag;
    }

    public Vector3 HitPos { get; }
    public float Range { get; }
    public string TargetTag { get; }
}

/// <summary>
/// ダメージの処理のメッセージの送信をラップしたクラス
/// </summary>
public static class DamageMessageSender
{
    public static void SendMessageToPlayer(Vector3 hitPos, float range)
    {
        MessageBroker.Default.Publish(new DamageData(hitPos, range, "Player"));
    }

    public static void SendMessageToEnemy(Vector3 hitPos, float range)
    {
        MessageBroker.Default.Publish(new DamageData(hitPos, range, "Enemy"));
    }
}

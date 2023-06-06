using UnityEngine;

/// <summary>
/// プレイヤーと敵のダメージのデータの構造体
/// この構造体をメッセージングでやり取りすることでダメージの処理を行う
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
/// InputSystemの入力を切り替えるデータの構造体
/// この構造体をメッセージングでやり取りすることでInputSystemで使う入力の切り替えを行う
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
/// タイトルからインゲームに遷移させるための構造体
/// ゲームスタートボタンを押した際に送信される
/// インゲーム開始時をトリガーしたい場合はこのメッセージを受信する
/// </summary>
struct ToInGameTrigger
{
}

/// <summary>
/// プレイヤーのダメージを受ける前と後の体力の構造体
/// プレイヤーがダメージを受けた場合にメッセージングされる
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
/// 表示する台詞の構造体
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
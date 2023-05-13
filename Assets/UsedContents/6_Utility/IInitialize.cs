using UnityEngine;

// 2つとも未使用

/// <summary>
/// Awakeのタイミングで初期化する処理があることを明示するためのインターフェース
/// 実装したクラスもしくは、そのクラスをモジュールとして持つMonoBehaviorのAwake()で呼ぶ想定
/// </summary>
public interface IInitializeOnAwake
{
    public void InitOnAwake(GameObject gameObject = null);
}

/// <summary>
/// Startのタイミングで初期化する処理があることを明示するためのインターフェース
/// 実装したクラスもしくは、そのクラスをモジュールとして持つMonoBehaviorのStart()で呼ぶ想定
/// </summary>
public interface IInitializeOnStart
{
    public void InitOnStart(GameObject gameObject = null);
}
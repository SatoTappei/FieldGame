using System;
using System.Buffers;
using UnityEngine;

/// <summary>
/// .NetStandard2.1の新機能を試す
/// </summary>
public class DotNetStandard2Dot1 : MonoBehaviour, IExampleInterface
{
    void Start()
    {
        ArrayPoolExample();
    }

    /// <summary>
    /// 一時的なバッファが頻繁に作成・破棄する場合に使う
    /// メモリアロケーションとGCコストの削減が期待できる。
    /// </summary>
    void ArrayPoolExample()
    {
        //using System;
        //using System.Buffers;

        // 最低512の長さ"以上"の配列をプールから借りる
        int[] buffer = ArrayPool<int>.Shared.Rent(512);
        Debug.Log(buffer.Length); // 512

        // 必要なサイズ(StartからLengthの長さ)をSpanで取得して1で埋める
        Span<int> span = buffer.AsSpan(0, 256);
        span.Fill(1);

        // 1が256回表示される
        foreach (int i in span)
        {
            Debug.Log(i);
        }

        // Return()する際に初期化されないので参照型はClear()すること
        // メモリリークの原因になる
        span.Clear();

        // 初期化したので0が256回表示される
        foreach (int i in span)
        {
            Debug.Log(i);
        }

        // 借りたら返す
        ArrayPool<int>.Shared.Return(buffer);
    }

    /// <summary>
    /// 配列の範囲構文
    /// ..で繋ぐとその範囲の要素が、^を付けると後ろから指定できる
    /// </summary>
    void RangeStructExample()
    {
        int[] array1 = new int[5] { 1, 2, 3, 4, 5 };
        int[] array2 = array1[0..2];
        int[] array3 = array1[1..^0];
    }
}

/// <summary>
/// InterfaceにDefaultの実装を持たせることが出来るようになった
/// 使い方がよくわからない、実装しない状態でメソッドを呼び出すことは出来ないらしい。
/// </summary>
public interface IExampleInterface
{
    public void ExampleMethod() => Debug.Log("InterfaceにDefaultの実装を持たせることが出来る");
}
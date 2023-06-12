using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メソッドを呼び出すだけで任意の文字がTextMeshでふわぁする
/// </summary>
public class PopTextMeshModule : MonoBehaviour
{
    static readonly int PoolQuantity = 10;

    [Header("設定済みのTextMeshのPrefab")]
    [SerializeField] PooledTextMesh _prefab;

    Stack<PooledTextMesh> _pool = new(PoolQuantity);

    public void Awake()
    {
        CreatePool();
    }

    /// <summary>
    /// プール用のオブジェクトを生成し、その子としてTextMeshを生成する
    /// </summary>
    void CreatePool()
    {
        GameObject parent = new GameObject("TextMeshPool");
        for (int i = 0; i < PoolQuantity; i++)
        {
            PooledTextMesh instance = Instantiate(_prefab, Vector3.zero, Quaternion.identity);

            // 最初の1つはプールを登録する
            if (i == 0) instance.RegisterPool(this);

            Push(instance);
            instance.transform.SetParent(parent.transform);
        }
    }

    /// <summary>
    /// 外部からこのメソッドを呼ぶだけで大丈夫
    /// 表示する文字列と座標を設定して取り出す
    /// </summary>
    public void Pop(string line, Vector3 worldPos)
    {
        if (_pool.TryPop(out PooledTextMesh instance))
        {
            instance.OnPoped(line, worldPos);
            instance.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"TextMeshが足りない: {line}");
        }
    }

    public void PopDefaultPos(string line) => Pop(line, new Vector3(0, 2, 3));

    public void Push(PooledTextMesh instance)
    {
        instance.gameObject.SetActive(false);
        _pool.Push(instance);
    }
}

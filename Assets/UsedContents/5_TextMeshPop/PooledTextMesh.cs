using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// プールされたTextMesh
/// ふわぁっていう感じのアニメーションをする
/// ポーズ処理にも対応している
/// </summary
public class PooledTextMesh : MonoBehaviour
{
    static PopTextMeshModule Pool;
    static readonly float LifeTime = 4.5f;
    static readonly Vector3 Destination = Vector3.up * 2;

    Transform _transform;
    TextMesh _textMesh;
    Vector3 _defaultPos;
    float _timer;

    void Awake()
    {
        _transform = transform;
        // プール側で表示する文字列や座標を弄っているのでTextMesh型が必要
        _textMesh = GetComponentInChildren<TextMesh>();
    }

    void OnEnable()
    {
        _defaultPos = transform.position;
    }

    void Update()
    {
        Billboard();
        Animation();
        CountTime();
    }

    /// <summary>
    /// staticなフィールドへ代入する処理なので
    /// 最初に生成されるインスタンスに対してのみ呼ばれる
    /// </summary>
    public void RegisterPool(PopTextMeshModule pool) => Pool = pool;

    public void OnPoped(string line, Vector3 worldPos)
    {
        _textMesh.text = line;
        _transform.position = worldPos;
    }

    void Billboard()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.y = _transform.position.y;
        transform.LookAt(cameraPos);
    }

    void CountTime()
    {
        // この値を0にするとタイマーが止まるのでポーズ出来る
        // アニメーションもタイマー変数を参照しているので0にすれば止まる
        float TimeScale = 1f;

        _timer += Time.deltaTime * TimeScale;
        if (_timer > LifeTime)
        {
            Pool.Push(this);
            _timer = 0;
        }
    }

    /// <summary>
    /// ふわぁってする処理
    /// タイマーの値を生存時間にRemapしているので生存時間ピッタリに座標の移動が完了する
    /// </summary>
    void Animation()
    {
        float t = math.remap(0, LifeTime, 0, 1, _timer);
        _transform.position = Vector3.Slerp(_defaultPos, _defaultPos + Destination, t);
    }

    void OnDestroy()
    {
        // ゲーム終了時に破棄されるのでその際にnullにする
        Pool = null;
    }
}

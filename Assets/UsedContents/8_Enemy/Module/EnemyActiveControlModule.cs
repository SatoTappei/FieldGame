using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

/// <summary>
/// 敵が視認可能かどうかのフラグを制御するクラス
/// プレイヤーと自身の距離を測って一定距離以下だったら視認可能とし
/// BehaviorTree側で毎フレームの更新処理が行われる
/// </summary>
[System.Serializable]
public class EnemyActiveControlModule
{
    static readonly float Interval = 0.5f;
    static readonly float Distance = 8.0f;

    [Header("このRendererがカメラに映っているかで判定する")]
    [SerializeField] Renderer _renderer;

    /// <summary>
    /// このフラグを参照してプレイヤーとの距離に応じた更新するしないを切り替える
    /// </summary>
    public bool IsActive { get; private set; } = true;

    public void InitOnAwake(Transform transform, Transform player)
    {
        // 複数の敵が同じタイミングで処理すると重くなるので開始時間をずらす
        System.TimeSpan due = System.TimeSpan.FromSeconds(Random.Range(0, 1.0f));

        // 一定間隔でプレイヤーとの距離を計算し、フラグを操作する
        Observable.Timer(due, System.TimeSpan.FromSeconds(Interval)).Subscribe(_ =>
        {
            float dist = Distance * Distance * Distance * Distance;
            IsActive = Vector3.SqrMagnitude(player.position - transform.position) < dist;
        }).AddTo(transform);
    }

    public void DrawActiveRange(Transform transform)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Distance * Distance);
    }
}
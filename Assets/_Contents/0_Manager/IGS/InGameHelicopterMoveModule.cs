using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// 脱出地点にヘリコプターがやってくる演出を制御するクラス
/// </summary>
[System.Serializable]
public class InGameHelicopterMoveModule
{
    static readonly float Speed = 6.0f;
    /// <summary>
    /// 目的地に到着したとみなす距離
    /// この値は移動速度を速くした場合、調整しないといけない
    /// </summary>
    static readonly float Approximately = 0.3f;

    [SerializeField] Transform _helicopter;
    [SerializeField] Transform _helicopterModel;
    [Header("ヘリコプターが到着する位置")]
    [SerializeField] Transform _heliRendezvousPoint;

    public async UniTask Execute(CancellationToken token)
    {
        // TODO:ヘリとポイントが同じ高さじゃないと上下斜めを向いた状態になってしまう
        Vector3 diff = _heliRendezvousPoint.position - _helicopter.position;
        _helicopterModel.forward = diff;

        while(diff.sqrMagnitude >= Approximately)
        {
            token.ThrowIfCancellationRequested();

            diff = _heliRendezvousPoint.position - _helicopter.position;
            _helicopter.Translate(diff.normalized * Time.deltaTime * Speed);
            await UniTask.Yield(token);
        }
    }
}

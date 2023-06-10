using UnityEngine;
using UnityEngine.Events;

public class MonkeyModelCallbackRegister : MonoBehaviour
{
    [Header("移動開始時に1回呼ばれる")]
    [SerializeField] UnityEvent _onMoveEnter;
    [Header("移動終了時に1回呼ばれる")]
    [SerializeField] UnityEvent _onMoveExit;
    [Header("攻撃を行うタイミングで呼ばれる")]
    [SerializeField] UnityEvent _onFire;
    [Header("撃破された際に呼ばれる")]
    [SerializeField] UnityEvent _onDefeated;

    public UnityEvent OnMoveEnter => _onMoveEnter;
    public UnityEvent OnMoveExit => _onMoveExit;
    public UnityEvent OnFire => _onFire;
    public UnityEvent OnDefeated => _onDefeated;
}

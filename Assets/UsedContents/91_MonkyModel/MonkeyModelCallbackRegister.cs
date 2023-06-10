using UnityEngine;
using UnityEngine.Events;

public class MonkeyModelCallbackRegister : MonoBehaviour
{
    [Header("�ړ��J�n����1��Ă΂��")]
    [SerializeField] UnityEvent _onMoveEnter;
    [Header("�ړ��I������1��Ă΂��")]
    [SerializeField] UnityEvent _onMoveExit;
    [Header("�U�����s���^�C�~���O�ŌĂ΂��")]
    [SerializeField] UnityEvent _onFire;
    [Header("���j���ꂽ�ۂɌĂ΂��")]
    [SerializeField] UnityEvent _onDefeated;

    public UnityEvent OnMoveEnter => _onMoveEnter;
    public UnityEvent OnMoveExit => _onMoveExit;
    public UnityEvent OnFire => _onFire;
    public UnityEvent OnDefeated => _onDefeated;
}

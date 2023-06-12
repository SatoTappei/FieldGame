using UnityEngine;

/// <summary>
/// Play���[�h�ɓ��������ɔ�\���ɂ�����N���X
/// ��Ƀf�o�b�O�p
/// </summary>
public class DisableOnPlayModeSystem : MonoBehaviour
{
    [Header("Renderer������\���ɂȂ�̂Œ���")]
    [SerializeField] Renderer[] _objects;

    void Start()
    {
        foreach(Renderer v in _objects)
        {
            v.enabled = false;
        }
    }
}

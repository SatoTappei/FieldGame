using UnityEngine;
using Unity.Mathematics;

/// <summary>
/// �v�[�����ꂽTextMesh
/// �ӂ킟���Ă��������̃A�j���[�V����������
/// �|�[�Y�����ɂ��Ή����Ă���
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
        // �v�[�����ŕ\�����镶�������W��M���Ă���̂�TextMesh�^���K�v
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
    /// static�ȃt�B�[���h�֑�����鏈���Ȃ̂�
    /// �ŏ��ɐ��������C���X�^���X�ɑ΂��Ă̂݌Ă΂��
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
        // ���̒l��0�ɂ���ƃ^�C�}�[���~�܂�̂Ń|�[�Y�o����
        // �A�j���[�V�������^�C�}�[�ϐ����Q�Ƃ��Ă���̂�0�ɂ���Ύ~�܂�
        float TimeScale = 1f;

        _timer += Time.deltaTime * TimeScale;
        if (_timer > LifeTime)
        {
            Pool.Push(this);
            _timer = 0;
        }
    }

    /// <summary>
    /// �ӂ킟���Ă��鏈��
    /// �^�C�}�[�̒l�𐶑����Ԃ�Remap���Ă���̂Ő������ԃs�b�^���ɍ��W�̈ړ�����������
    /// </summary>
    void Animation()
    {
        float t = math.remap(0, LifeTime, 0, 1, _timer);
        _transform.position = Vector3.Slerp(_defaultPos, _defaultPos + Destination, t);
    }

    void OnDestroy()
    {
        // �Q�[���I�����ɔj�������̂ł��̍ۂ�null�ɂ���
        Pool = null;
    }
}

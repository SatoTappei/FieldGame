using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �䎌��\������UI�𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class LineTextModule
{
    [SerializeField] Text _lineText;
    [Header("�䎌��\�����Ă��鎞��")]
    [SerializeField] float _lifeTime = 2.0f;

    float _time;

    public void InitOnAwake(GameObject gameObject)
    {
        // ���������͂��ꂽ��ԂŎn�܂�̂�h��
        _lineText.text = string.Empty;

        // ��M������^�C�}�[�����Z�b�g���đ䎌���X�V
        MessageBroker.Default.Receive<LineData>().Subscribe(data =>
        {
            ResetLineText();

            // TODO:�F�̎w��p�̕������萔�ɕێ�������@
            _lineText.text = $"<color=#FF4C1C>{data.Name}</color> : {data.Line}";
        }).AddTo(gameObject);
    }

    public void Update()
    {
        if (_lineText.text == string.Empty) return;

        _time += Time.deltaTime;
        if (_time > _lifeTime)
        {
            _time = 0;
            _lineText.text = string.Empty;
        }
    }

    // TODO: �O������\�����ꂽ�Z���t�������I�ɏ��������ꍇ��public�ɂ���
    void ResetLineText()
    {
        _time = 0;
        _lineText.text = string.Empty;
    }
}

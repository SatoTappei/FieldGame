using Cysharp.Text;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �C���Q�[�����̃^�C�}�[�𐧌䂷��N���X
/// </summary>
[System.Serializable]
public class InGameTimerModule
{
    [SerializeField] Text _text;
    [Header("��������")]
    [Tooltip("�b"), Range(0, 60)]
    [SerializeField] int _seconds = 0;
    [Tooltip("��")]
    [SerializeField] int _minute = 3;

    /// <summary>
    /// ���̒l����deltaTime���������l�𕪂ƕb�ɒ����ĕ\������
    /// </summary>
    float _time;
    /// <summary>
    /// �b���X�V���ꂽ�ꍇ�ɂ̂݃e�L�X�g��ύX����悤�O�̕b��ێ����Ă���
    /// </summary>
    int _prevSeconds;

    public void InitOnAwake()
    {
        _time = _minute * 60 + _seconds;
        _prevSeconds = _seconds;
    }

    /// <summary>
    /// �Ăяo�����ƂŃ^�C�}�[���X�V����̂ŌĂяo���Ȃ���Έꎞ��~�Ɠ��������ɂȂ�
    /// </summary>
    public void Update()
    {
        if (_time <= 0) return;

        _time -= Time.deltaTime;
        int m = (int)_time / 60;
        int s = (int)(_time - m * 60);

        if (_prevSeconds != s)
        {
            _text.text = ZString.Concat($"{m:00}:{s:00}");
        }

        _prevSeconds = s;
    }
}

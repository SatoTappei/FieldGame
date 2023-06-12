using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// �v���C���[�̗̑͂�\������UI
/// </summary>
[System.Serializable]
public class LifePointGaugeModule
{
    [SerializeField] Image[] _icons;
    [Header("�A�C�R���̐F�̐ݒ�(�f�t�H���g/�_���[�W)")]
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _damagedColor;
    [Header("�v���C���[�̗̑͂Ɠ����l��ݒ肷��")]
    [SerializeField] int _playerMaxLifePoint;

    public void InitOnAwake(GameObject gameObject)
    {
        MessageBroker.Default.Receive<PlayerLifePointData>().Subscribe(data =>
        {
            float value = _icons.Length * (data.AfterValue * 1.0f / _playerMaxLifePoint * 1.0f);
            for (int i = 0; i < _icons.Length; i++)
            {
                _icons[i].color = i < value ? _defaultColor : _damagedColor;
            }
        }).AddTo(gameObject);
    }
}

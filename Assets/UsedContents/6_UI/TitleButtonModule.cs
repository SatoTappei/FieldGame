using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// �^�C�g���̃{�^�����̏����̃N���X
/// </summary>
[System.Serializable]
public class TitleButtonModule
{
    [SerializeField] Button _button;
    [SerializeField] Image _cursor;

    /// <summary>
    /// �N���b�N���̏��������O������ǉ��ł���悤�Ɍ��J����
    /// </summary>
    public event UnityAction OnSubmit;

    public void InitOnStart()
    {
        _button.OnSelectAsObservable().Subscribe(_ => OnSelect());
        _button.OnDeselectAsObservable().Subscribe(_ => OnDeselect());
        _button.onClick.AddListener(Submit);

        _cursor.color = Color.clear;
    }

    /// <summary>
    /// ����炵�ăR�[���o�b�N�����s���邾����
    /// �e�퉉�o��ǉ������蓯��������悤�ɂ͂Ȃ��Ă��Ȃ�
    /// </summary>
    void Submit()
    {
        OnSubmit.Invoke();
        GameManager.Instance?.AudioModule.PlaySE(AudioType.SE_ButtonSubmit);
    }

    void FadeIn()
    {
    }

    void FadeOut()
    {
    }

    void OnSelect()
    {
        _button.transform.localScale = Vector3.one * 1.1f;
        _button.image.color = _button.colors.selectedColor;
        _cursor.color = Color.white;

        GameManager.Instance?.AudioModule.PlaySE(AudioType.SE_ButtonSelect);
    }

    void OnDeselect()
    {
        _button.transform.localScale = Vector3.one;
        _button.image.color = _button.colors.normalColor;
        _cursor.color = Color.clear;
    }
}

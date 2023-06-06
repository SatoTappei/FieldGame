using UniRx;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 台詞を表示するUIを制御するクラス
/// </summary>
[System.Serializable]
public class LineTextModule
{
    [SerializeField] Text _lineText;
    [Header("台詞を表示している時間")]
    [SerializeField] float _lifeTime = 2.0f;

    float _time;

    public void InitOnAwake(GameObject gameObject)
    {
        // 文字が入力された状態で始まるのを防ぐ
        _lineText.text = string.Empty;

        // 受信したらタイマーをリセットして台詞を更新
        MessageBroker.Default.Receive<LineData>().Subscribe(data =>
        {
            ResetLineText();

            // TODO:色の指定用の文字列を定数に保持する方法
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

    // TODO: 外部から表示されたセリフを強制的に消したい場合はpublicにする
    void ResetLineText()
    {
        _time = 0;
        _lineText.text = string.Empty;
    }
}

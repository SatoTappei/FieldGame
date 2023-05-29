using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 再生する音を指定するための列挙型
/// 音を追加したらここにも対応する値を追加する
/// </summary>
public enum AudioType
{
    SE_Fire,
    SE_ButtonSubmit,
    SE_ButtonSelect,
    SE_PlayerDamage,
}

/// <summary>
/// 音の再生を行うクラス
/// </summary>
[System.Serializable]
public class AudioModule
{
    /// <summary>
    /// インスペクターから割り当てるための音のデータのクラス
    /// </summary>
    [System.Serializable]
    public class AudioData
    {
        [SerializeField] AudioType _key;
        [SerializeField] AudioClip _clip;
        [SerializeField] float _volume;

        public AudioType Key => _key;
        public AudioClip Clip => _clip;
        public float Volume => _volume;
    }

    [SerializeField] AudioData[] _audioDatas;
    [Header("同時に再生できる数")]
    [SerializeField] int _playAtSame = 10;
    [Header("連続で再生できる間隔")]
    [SerializeField] float _interval = 0.05f;

    AudioSource[] _audioSources;
    Dictionary<AudioType, AudioData> _audioDict = new();
    float _lastTime;

    public void InitOnAwake(GameObject go)
    {
        _audioSources = new AudioSource[_playAtSame];

        for (int i = 0; i < _audioSources.Length; i++)
        {
            _audioSources[i] = go.AddComponent<AudioSource>();
        }

        foreach (AudioData data in _audioDatas)
        {
            _audioDict.Add(data.Key, data);
        }
    }

    public void PlaySE(AudioType type)
    {
        if (IsInterval()) return;

        AudioData data = GetData(type);
        AudioSource source = GetSourceSE();
        if (data == null || source == null) return;

        source.clip = data.Clip;
        source.volume = data.Volume;
        source.Play();
    }

    public void PlayBGM()
    {
        // 処理を追加する
    }

    bool IsInterval()
    {
        if (Time.realtimeSinceStartup - _lastTime > _interval)
        {
            _lastTime = Time.realtimeSinceStartup;
            return false;
        }
        else
        {
            return true;
        }
    }

    AudioSource GetSourceSE()
    {
        // 一番後ろのAudioSourceはBGM再生用なので取っておく
        for (int i = 0; i < _audioSources.Length - 1; i++)
        {
            if (!_audioSources[i].isPlaying) return _audioSources[i];
        }

        Debug.LogWarning("AudioSourceが不足");
        return null;
    }

    AudioData GetData(AudioType type)
    {
        if (_audioDict.TryGetValue(type, out AudioData data))
        {
            return data;
        }

        throw new KeyNotFoundException($"音が登録されていない: {type}");
    }
}
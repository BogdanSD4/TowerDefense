using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType
{
    Sniper,
    Zombie_2,
    Zombie_3,
    LoseSound,
    WinSound,
    StartWave
}
public enum MusicType
{
    WaveMusic,
    MusicBeforeWave,
    Water,
    MachineGun
}
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;

    [SerializeField] private List<SfxData> _sfxDatas = new List<SfxData>();
    [SerializeField] private List<AudioData> _audioDatas = new List<AudioData>();

    private static AudioManager _inctanse;
    private void Awake()
    {
        if(_inctanse == null)
        {
            _inctanse = this;
        }
        DontDestroyOnLoad(_inctanse);
    }
    private void PlayMusicInner(MusicType musicType)
    {
        var musicData = _audioDatas.Find(data => data.Type == musicType);
        if(_audioDatas != null)
        {
            _musicSource.clip = musicData.Music;
            _musicSource.Play();
        }
    }
    private void PlaySFXInner(SFXType sFX)
    {
        var sfxData = _sfxDatas.Find(data => data.SFX == sFX);
        if(sfxData != null)
        {
            _sfxSource.PlayOneShot(sfxData.Audio);
        }
    }
    public static void PlaySFX(SFXType sFX)
    {
        _inctanse.PlaySFXInner(sFX);
    }
    public static void PlayAudio(MusicType music)
    {
        _inctanse.PlayMusicInner(music);
    }
}
[System.Serializable]
public class SfxData
{
    [SerializeField] private SFXType _sFX;
    [SerializeField] private AudioClip _audioClip;

    public SFXType SFX => _sFX;

    public AudioClip Audio => _audioClip;
}
[System.Serializable]
public class AudioData
{
    [SerializeField] private MusicType _musicType;
    [SerializeField] private AudioClip _clip;

    public MusicType Type => _musicType;

    public AudioClip Music => _clip;
}

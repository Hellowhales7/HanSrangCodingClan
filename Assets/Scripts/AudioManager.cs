using System.Runtime.CompilerServices;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    private AudioSource _bgmPlayer;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    private AudioSource[] _sfxPlayers;
    private int _channelIndex;

    public enum Sfx
    {
        CR,
        RCR,
        Start,
        BreakBlock,
        HardDrop,
        Right,
        Left,
        Word_Correct,
        Word_No_element,
        word_refill,
        word_wrong
    };

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Init()
    {
        var bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        _bgmPlayer = bgmObject.AddComponent<AudioSource>();
        _bgmPlayer.playOnAwake = false;
        _bgmPlayer.loop = true;
        _bgmPlayer.volume = bgmVolume;
        _bgmPlayer.clip = bgmClip;

        var sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        _sfxPlayers = new AudioSource[channels];

        for (var index = 0; index < _sfxPlayers.Length; index++)
        {
            _sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            _sfxPlayers[index].playOnAwake = false;
            _sfxPlayers[index].volume = sfxVolume;
        }

        PlaySfx(Sfx.Start);
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            _bgmPlayer.Play();
        }
        else
        {
            _bgmPlayer.Stop();
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        Debug.Log($"[KW] PlaySfx: {sfx}");

        for (var index = 0; index < _sfxPlayers.Length; index++)
        {
            Debug.Log($"index: {index}");

            var loopIndex = (index + _channelIndex) % _sfxPlayers.Length;
            
            Debug.Log($"loopIndex: {loopIndex}");

            if (_sfxPlayers[loopIndex].isPlaying)
            {
                Debug.Log($"_sfxPlayers[loopIndex].isPlaying: {_sfxPlayers[loopIndex].isPlaying}");
                continue;
            }

            _channelIndex = loopIndex;
            _sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            _sfxPlayers[loopIndex].Play();

            break;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;

public class AudioManager : MonoBehaviour
{
    public class AudioSourceAndObject
    {
        public AudioSource AudioSource;
        public GameObject AudioObject;
    }
    [Range(0f, 2f)]
    [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundsCollectionSO _soundsCollectionSO;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private ObjectPool<AudioSourceAndObject> _audioSourcePool;
    private AudioSource _currentMusic;
    private int ENEMY_HITABLE_COLLIDERS_LAYER;

    #region Unity Methods

    private void Start()
    {
        CreateAudioSourcePool();
        ENEMY_HITABLE_COLLIDERS_LAYER = LayerMask.NameToLayer("EnemyHitableColliders");
    }

    private void OnEnable()
    {
        CharacterAnimation.OnMovementChange += CharacterAnimation_OnMovementChange;
    }

    private void OnDisable()
    {
        CharacterAnimation.OnMovementChange -= CharacterAnimation_OnMovementChange;
    }

    private void CreateAudioSourcePool()
    {
        _audioSourcePool = new ObjectPool<AudioSourceAndObject>(() =>
        {
            GameObject audioObject = new GameObject("Temp Audio Source");
            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            return new AudioSourceAndObject { AudioSource = audioSource, AudioObject = audioObject };
        }, audioSourceAndObject =>
        {
            audioSourceAndObject.AudioObject.SetActive(true);
        }, audioSourceAndObject =>
        {
            audioSourceAndObject.AudioObject.SetActive(false);
        }, audioSourceAndObject =>
        {
            Destroy(audioSourceAndObject.AudioObject);
        }, false, 20);
    }

    private IEnumerator ReleaseAfterDelay(AudioSourceAndObject audioSourceAndObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        _audioSourcePool.Release(audioSourceAndObject);
    }

    #endregion

    #region Sound Methods

    private void SoundToPlay(SoundSO audioSO)
    {
        AudioClip clip = audioSO.Clip;
        float pitch = audioSO.Pitch;
        float volume = audioSO.Volume * _masterVolume;
        bool loop = audioSO.Loop;
        AudioMixerGroup audioMixerGroup;
        pitch = RandomizePitch(audioSO, pitch);
        audioMixerGroup = DetermineAudioMixerGroup(audioSO);

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO audioSO)
    {
        AudioMixerGroup audioMixerGroup = audioSO.AudioType switch
        {
            SoundSO.AudioTypes.SFX => _sfxMixerGroup,
            SoundSO.AudioTypes.Music => _musicMixerGroup,
            _ => null,
        };
        return audioMixerGroup;
    }

    private float RandomizePitch(SoundSO audioSO, float pitch)
    {
        if (audioSO.RandomizePitch)
        {
            float randomPitchModifier = Random.Range(-audioSO.RandomPitchRangeModifier, audioSO.RandomPitchRangeModifier);
            pitch = audioSO.Pitch + randomPitchModifier;
        }

        return pitch;
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        AudioSourceAndObject audioSourceAndObject = _audioSourcePool.Get();
        AudioSource audioSource = audioSourceAndObject.AudioSource;
        audioSource.playOnAwake = false;
        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!loop)
        {
            StartCoroutine(ReleaseAfterDelay(audioSourceAndObject, audioSource.clip.length));
        }

        DetermineMusic(audioMixerGroup, audioSource);
    }

    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
    {
        if (audioMixerGroup != _musicMixerGroup) { return; }

        if (_currentMusic != null)
        {
            _currentMusic.Stop();
        }

        _currentMusic = audioSource;
    }

    private void PlayRandomSound(SoundSO[] audios)
    {
        if (audios == null || audios.Length == 0) { return; }
        SoundSO audioSO = audios[Random.Range(0, audios.Length)];
        SoundToPlay(audioSO);
    }

    #endregion

    #region SFX

    private void CharacterAnimation_OnMovementChange(bool obj)
    {
        if (obj)
            PlayRandomSound(_soundsCollectionSO.Running);
    }

    #endregion

    #region Music

    #endregion

    #region Custom SFX Logic

    #endregion
}
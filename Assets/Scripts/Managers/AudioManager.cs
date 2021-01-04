using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region  FIELDS DECLERATION

    private AudioSource firstMusicSource;
    private AudioSource secondMusicSource;
    private AudioSource sfxSource;

    private bool firstMusicSourceIsPlaying = false;

    #region STATIC INSTANCE

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("AudioManager Doesn't exist");
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    #endregion

    #endregion

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this.gameObject);

        //Create audio Source and save then as reference;
        firstMusicSource = gameObject.AddComponent<AudioSource>();
        secondMusicSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // loop music
        firstMusicSource.loop = true;
        secondMusicSource.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = firstMusicSourceIsPlaying ? firstMusicSource : secondMusicSource;

        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();
    }

    #region PLAY MUSIC WITH FADE EFFECT

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1f)
    {
        AudioSource activeSource = firstMusicSourceIsPlaying ? firstMusicSource : secondMusicSource;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }
    private IEnumerator UpdateMusicWithFade(AudioSource source, AudioClip newClip, float transitionTime)
    {
        if (!source.isPlaying)
        {
            source.Play();
        }

        float t = 0.0f;
        // Fade out
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            source.volume = 1 - (t / transitionTime);
            yield return null;
        }

        source.Stop();
        source.clip = newClip;
        source.Play();

        // Fade In
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            source.volume = (t / transitionTime);
            yield return null;
        }
    }

    #endregion

    #region PLAY MUSIC WITH CROSS FADE EFFECT

    public void PlayMusicWithCrossFade(AudioClip newClip, float transitionTime = 1f)
    {
        // determines which music source is active
        AudioSource activeSource = firstMusicSourceIsPlaying ? firstMusicSource : secondMusicSource;
        AudioSource newSource = firstMusicSourceIsPlaying ? secondMusicSource : firstMusicSource;

        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        newSource.clip = newClip;
        newSource.Play();

        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }
    
    private IEnumerator UpdateMusicWithCrossFade(AudioSource originalSource, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            originalSource.volume = 1 - (t / transitionTime);
            //newSource.volume = 1 - (t / transitionTime);
            newSource.volume = t / transitionTime;
            yield return null;
        }
        originalSource.Stop();
    }

    #endregion

    #region PLAY SFX

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    #endregion

    #region GENERIC CONTROLS

    public void SetMusicVolume(float volume)
    {
        firstMusicSource.volume = volume;
        secondMusicSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    #endregion

}
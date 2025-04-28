using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // Required for scene management


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource; // For background music
    public AudioSource sfxSource;   // For sound effects
    public AudioSource ambientSource; // For ambient sounds

    private Coroutine fadeCoroutine; // To manage fades 

    [Header("Audio Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    public AudioClip gameOverMusic;

    [Header("Audio Mixer and Snapshots")]
    public AudioMixer audioMixer; // Reference to the Audio Mixer
    public AudioMixerSnapshot defaultSnapshot; // Normal gameplay
    public AudioMixerSnapshot pausedSnapshot; // Paused state
    public AudioMixerSnapshot gameOverSnapshot; // Game over state
    public AudioMixerSnapshot victorySnapshot; // Victory state

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, float volume = 1f, float fadeDuration = 1f)
    {
        if (musicSource.clip == clip) return; // Avoid restarting the same music

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Stop any ongoing fade
        }
        fadeCoroutine = StartCoroutine(FadeMusic(clip, volume, fadeDuration)); // Start fading
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayAmbient(AudioClip clip, float volume = 1f)
    {
        ambientSource.clip = clip;
        ambientSource.volume = volume;
        ambientSource.loop = true;
        ambientSource.Play();
    }

    public void StopAmbient()
    {
        ambientSource.Stop();
    }

    public void SetVolume(string source, float volume)
    {
        switch (source.ToLower())
        {
            case "music":
                musicSource.volume = volume;
                break;
            case "sfx":
                sfxSource.volume = volume;
                break;
            case "ambient":
                ambientSource.volume = volume;
                break;
            default:
                Debug.LogWarning($"Unknown audio source: {source}");
                break;
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip, float targetVolume, float duration)
    {
        // Fade out current music
        float startVolume = musicSource.volume;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in new music
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0, targetVolume, t / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }

    public void TransitionToSnapshot(AudioMixerSnapshot snapshot, float transitionTime = 1f)
    {
        if (snapshot != null)
        {
            snapshot.TransitionTo(transitionTime);
        }
        else
        {
            Debug.LogWarning("Snapshot is null. Cannot transition.");
        }
    }
}

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

    private Dictionary<string, List<AudioClip>> tagToAudioClips = new Dictionary<string, List<AudioClip>>();



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

        // Load audio clips for ricochet sounds
        LoadAudioClipsForTag("Wall", "Audio/SFX/Ricochet/Wall/");
        LoadAudioClipsForTag("Ground", "Audio/SFX/Ricochet/Ground/");
        LoadAudioClipsForTag("Enemy", "Audio/SFX/Ricochet/Enemy/");
        LoadAudioClipsForTag("Player", "Audio/SFX/Ricochet/Player/");
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

    public void PlaySFX(AudioClip clip, float volume = 1f, Vector3? position = null)
    {
        if (clip == null)
        {
            Debug.LogWarning("Attempted to play a null AudioClip.");
            return;
        }
        if (position.HasValue)
        {
            // Create a temporary GameObject for 3D sound
            GameObject audioObject = new GameObject("SFX_3D");
            audioObject.transform.position = position.Value;

            AudioSource audioSource = audioObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = 1.0f; // 3D sound
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 50f;

            audioSource.Play();
            Destroy(audioObject, clip.length);
        }
        else
        {
            // Play 2D sound
            sfxSource.PlayOneShot(clip, volume);
        }

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

    private void LoadAudioClipsForTag(string tag, string folderPath)
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderPath);
        if (clips.Length > 0)
        {
            tagToAudioClips[tag] = new List<AudioClip>(clips);
            Debug.Log($"Loaded {clips.Length} audio clips for tag: {tag} from folder: {folderPath}");
        }
        else
        {
            Debug.LogError($"No audio clips found in folder: {folderPath} for tag: {tag}");
            
        }
    }

    public void ReloadAudioClips()
    {
        tagToAudioClips.Clear(); // Clear existing clips
        LoadAudioClipsForTag("Wall", "Audio/SFX/Ricochet/Wall/");
        LoadAudioClipsForTag("Ground", "Audio/SFX/Ricochet/Ground/");
        LoadAudioClipsForTag("Enemy", "Audio/SFX/Ricochet/Enemy/");
        LoadAudioClipsForTag("Player", "Audio/SFX/Ricochet/Player/");
        Debug.Log("Reloaded audio clips for ricochet sounds.");
    }

    public void PlayRicochetSound(Vector3 position, string tag)
    {
        if (tagToAudioClips.TryGetValue(tag, out List<AudioClip> clips) && clips.Count > 0)
        {
            // Randomly select an audio clip
            AudioClip clip = clips[Random.Range(0, clips.Count)];

            // Use AudioManager to play the sound
            AudioManager.Instance.PlaySFX(clip, 1f);
            Debug.Log($"Playing ricochet sound for tag: {tag} at position: {position}");
        }
        else
        {
            Debug.LogWarning($"No audio clips available for tag: {tag}");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// Singleton responsible for routing all in-game audio through category-based sub-managers.
// Each category (music, SFX, UI, …) gets its own AudioGroupManager so volumes can be
// tweaked independently and clips can be pooled or faded consistently.
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region Fields and References
    /// <summary>
    /// ScriptableObject that stores mixer-routing info and the user’s volume preferences.
    /// </summary>
    [SerializeField, Expandable] private VolumeSettings volumeSettings;

    /// <summary>
    /// Lookup table: one AudioGroupManager per AudioCategory.
    /// </summary>
    private Dictionary<AudioCategory,AudioGroupManager> audioGroups;

    /// <summary>Convenience accessor that never throws if the key doesn’t exist.</summary>
    private AudioGroupManager GetChannelByCategory(AudioCategory category)
    {
        return audioGroups.GetValueOrDefault(category);
    }
    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        // Create one AudioGroupManager for every VolumeGroupControl defined
        // in the VolumeSettings asset. Doing this up-front avoids runtime allocations.
        audioGroups = new(); audioGroups = new();
        foreach (KeyValuePair<AudioCategory, VolumeGroupControl> v in volumeSettings.groups)
        {
            audioGroups.Add(
                v.Key,
                new AudioGroupManager(
                    this,               // Owning AudioManager instance
                    v.Key,              // Category handled by this manager
                    v.Value.audioMode,  // 2D, 3D or Spatial Blend
                    v.Value.Type,       // Pooling / One-Shot / Persistent
                    v.Value.Group,      // Target AudioMixerGroup
                    v.Value.loop));     // Should the clip loop?
        }
    }

    private void Start()
    {
        // Restore volume sliders from disk (PlayerPrefs, JSON, …).
        volumeSettings?.LoadVolumeSettings();
    }

    private void OnDestroy()
    {
        // Persist volume sliders when the AudioManager is unloaded.
        //volumeSettings?.SaveVolumeSettings();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Play a clip under the given category. Optional fade-in/out, custom pitch,
    /// and 3D position are supported.
    /// </summary>
    public AudioSource Play(AudioCategory category, AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeInTime = 0f, float fadeOutTime = 0f, Vector3 position = default)
    {
        return GetChannelByCategory(category).PlayAudio(clip,targetVolume,targetPitch,fadeInTime,fadeOutTime,position);
    }
    #endregion

    #region Pause, Resume, Stop all audios in a category.
    /// <summary>Fade-out then pause every AudioSource in the category.</summary>
    public void Pause(AudioCategory category, float fadeTime = 0f)
    {
        GetChannelByCategory(category).PauseAudio(fadeTime);
    }

    /// <summary>Fade-in then resume every AudioSource in the category.</summary>
    public void Resume(AudioCategory category, float fadeTime = 0f)
    {
        GetChannelByCategory(category).ResumeAudio(fadeTime);
    }

    /// <summary>Fade-out then stop (destroy) every AudioSource in the category.</summary>
    public void Stop(AudioCategory category, float fadeTime = 0f)
    {
        GetChannelByCategory(category).StopAudio(fadeTime);
    }
    #endregion

    #region Pause, Resume, Stop all audios in all categories.
    /// <summary>Fade-out & pause all managed AudioCategories.</summary>
    public void PauseAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioGroupManager> entry in audioGroups)
        {
            entry.Value.PauseAudio(fadeTime);
        }
    }

    /// <summary>
    /// Immediate pause of every AudioSource in the scene, including those the AudioManager
    /// doesn’t know about (cut-scenes, Timeline tracks, etc.).
    /// </summary>
    public void PauseAllAudioForced()
    {
        AudioSource[] sources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach(AudioSource source in sources)
        {
            source.Pause();
        }
    }

    /// <summary>Fade-in & resume all managed AudioCategories.</summary>
    public void ResumeAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioGroupManager> entry in audioGroups)
        {
            entry.Value.ResumeAudio(fadeTime);
        }
    }

    /// <summary>Immediate un-pause of every AudioSource in the scene.</summary>
    public void ResumeAllAudioForced()
    {
        AudioSource[] sources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in sources)
        {
            source.UnPause();
        }
    }
    /// <summary>Immediate stop of every AudioSource in the scene.</summary>
    public void StopAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioGroupManager> entry in audioGroups)
        {
            entry.Value.StopAudio(fadeTime);
        }
    }

    public void StopAllAudioForced()
    {
        AudioSource[] sources = GameObject.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);

        foreach (AudioSource source in sources)
        {
            source.Stop();
        }
    }
    #endregion

    #region Static methods
    /// <summary>
    /// Play a 3D one-shot sound at <paramref name="position"/> without the need for a
    /// pre-existing AudioSource. A temporary GameObject is created and destroyed automatically.
    /// </summary>
    public static AudioSource PlaySoundAtPoint(AudioClip clip, float volume, float pitch, float fadeInTime = 0f, float fadeOutTime = 0f, AudioMixerGroup mixerGroup = null, Vector3 position = default)
    {
        if (!clip) return null;

        GameObject tempGO = new GameObject("TempAudio_OneShot");
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        if (mixerGroup != null)
            tempSource.outputAudioMixerGroup = mixerGroup;

        tempSource.clip = clip;
        tempSource.pitch = pitch;

        tempSource.spatialBlend = (position != default) ? 1f : 0f; //ADDAPT


        if (fadeInTime > 0f || fadeOutTime > 0f)
        {
            // Instead of manually fading via AudioFadeUtility,
            // attach an AudioFade component that will handle fade-in.
            AudioFade fade = tempGO.AddComponent<AudioFade>();

            //Fade-in
            fade.fadeInOnStart = fadeInTime > 0f;
            fade.defaultFadeInDuration = fadeInTime;
            fade.targetVolume = volume;

            //Fade-out
            fade.fadeOutOnEnd = fadeOutTime > 0f;
            fade.defaultFadeOutDuration = fadeOutTime;

            // Do not call tempSource.Play() here—the AudioFade's Start() method will handle it.
            if(fadeInTime <= 0) tempSource.Play();
        }
        else
        {
            tempSource.volume = volume;
            tempSource.Play();
        }

        Destroy(tempGO, clip.length / Mathf.Abs(pitch));
        return tempSource;
    }
    #endregion

}

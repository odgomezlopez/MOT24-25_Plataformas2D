using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioCategory
{
    Background,
    Music,
    Dialogue,
    SFX
}

//Improvements: - SFXs using Audio Pool. - Pause and Resume SFXs
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region Fields and References

    [SerializeField, Expandable] private VolumeSettings volumeSettings;
    [SerializeField] private AudioDictionary generalAudioDictionary;
    public bool HasKey(AudioCategory category, string key) => generalAudioDictionary?.HasKey(category, key) ?? false;


    [Header("Audio Sources (separados para cada categoría)")]
    [SerializeField] private AudioSource backgroundAudioSource;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource dialogueAudioSource;
    // Note: SFX uses PlaySoundAtPoint (no dedicated AudioSource)

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        // Aseguramos que cada AudioSource existe; si no, lo creamos
        SetupAudioSource(ref backgroundAudioSource, "BackgroundAudioSource",volumeSettings.background.Group);
        SetupAudioSource(ref musicAudioSource, "MusicAudioSource", volumeSettings.music.Group);
        SetupAudioSource(ref dialogueAudioSource, "DialogueAudioSource", volumeSettings.dialogue.Group);
    }

    private void Start()
    {
        volumeSettings?.LoadVolumeSettings();
    }

    private void OnDestroy()
    {
        volumeSettings?.SaveVolumeSettings();
    }

    #endregion

    #region Public Methods
    public void PlayAudio(AudioCategory category, AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeTime = 0f, Vector3 position = default)
    {
        if (clip == null) return;
        PlayAudioByCategory(category, clip, targetVolume, targetPitch, fadeTime, position);
    }

    public void ChangeAudio(AudioCategory category, AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (clip == null) return;
        StartCoroutine(ChangeAudioInternal(category,clip,fadeOutTime,fadeInTime, targetVolume, targetPitch, position));
    }

    public void StopAudio(AudioCategory category, float fadeTime = 0f)
    {
        AudioSource source = GetSourceByCategory(category);
        if (!source || !source.isPlaying) return;

        if (fadeTime > 0f)
        {
            // Fade out, luego Stop
            AudioFadeUtility.FadeOut(this, source, fadeTime, () => source.Stop());
        }
        else
        {
            source.volume = 0f;
            source.Stop();
        }
    }

    public void PauseAudio(AudioCategory category, float fadeTime = 0f)
    {
        AudioSource source = GetSourceByCategory(category);
        if (!source || !source.isPlaying)
        {
            if(category == AudioCategory.SFX) Debug.LogWarning("PauseAudio called for SFX category. Pause/resume is not supported for temporary SFX audios.");
            return;
        } 
            
        if (fadeTime > 0f)
        {
            // Fade out, luego Pause
            AudioFadeUtility.FadeOut(this, source, fadeTime, () => source.Pause());
        }
        else
        {
            source.volume = 0f;
            source.Pause();
        }
    }
    public void ResumeAudio(AudioCategory category, float fadeTime = 0f)
    {
        AudioSource source = GetSourceByCategory(category);
        if (!source || !source.isPlaying)
        {
            if (category == AudioCategory.SFX) Debug.LogWarning("PauseAudio called for SFX category. Pause/resume is not supported for temporary SFX audios.");
            return;
        }

        // Reanuda, luego fade in
        source.UnPause();

        //Obtener el valor que tenia antes de la pausa. TODO hacer DICT que almacene el valor de cada Audio 
        float toVolume = 1f;

        if (fadeTime > 0f)
        {
            AudioFadeUtility.FadeIn(this, source, fadeTime, toVolume);
        }
        else
        {
            source.volume = toVolume;
        }
    }

    // Optional methods to pause/resume all non-SFX audio.
    public void PauseAllAudio(float fadeTime = 0f)
    {
        PauseAudio(AudioCategory.Background, fadeTime);
        PauseAudio(AudioCategory.Music, fadeTime);
        PauseAudio(AudioCategory.Dialogue, fadeTime);
    }

    public void ResumeAllAudio(float fadeTime = 0f)
    {
        ResumeAudio(AudioCategory.Background, fadeTime);
        ResumeAudio(AudioCategory.Music, fadeTime);
        ResumeAudio(AudioCategory.Dialogue, fadeTime);
    }

    /// <summary>
    /// Audio 3D one-shot sin un AudioSource persistente.
    /// </summary>
    public static AudioSource PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume, float pitch,
                                        AudioMixerGroup mixerGroup = null, float fadeInTime = 0f, float fadeOutTime = 0f)
    {
        if (!clip) return null;

        GameObject tempGO = new GameObject("TempAudio_OneShot");
        tempGO.transform.position = position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        if (mixerGroup != null)
            tempSource.outputAudioMixerGroup = mixerGroup;

        tempSource.clip = clip;
        tempSource.pitch = pitch;

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

    #region Play and Change Compability Methods
    public void PlayAudio(AudioCategory category, AudioClipSO clipSO, float fadeTime = 0f, Vector3 position = default)
    {
        if (clipSO == null) return;

        AudioClip clip = clipSO.GetRandomClip();
        if (!clip) return;

        float adjustedVolume = clipSO.GetAdjustedVolume();
        float adjustedPitch = clipSO.GetAdjustedPitch();
        PlayAudioByCategory(category, clip, adjustedVolume, adjustedPitch, fadeTime, position);
    }

    public void PlayAudio(AudioCategory category, string key, float fadeTime = 0f, Vector3 position = default)
    {
        if (generalAudioDictionary == null) return;

        AudioClipReference reference = generalAudioDictionary.GetClipReferenceByCategory(category, key);
        if (reference == null) return;

        PlayAudio(category, reference, fadeTime, position);
    }

    public void PlayAudio(AudioCategory category, AudioClipReference reference, float fadeTime = 0f, Vector3 position = default)
    {
        if (reference == null) return;

        if (reference.HasAudioClipSO)
        {
            PlayAudio(category, reference.ClipSO, fadeTime, position);
        }
        else
        {
            PlayAudio(category, reference.Clip, 1, 1, fadeTime, position);
        }
    }

    public void ChangeAudio(AudioCategory category, AudioClipSO clipSO, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (clipSO == null) return;

        AudioClip clip = clipSO.GetRandomClip();
        if (!clip) return;

        float adjustedVolume = clipSO.GetAdjustedVolume();
        float adjustedPitch = clipSO.GetAdjustedPitch();

        StartCoroutine(ChangeAudioInternal(category, clip, fadeOutTime, fadeInTime, adjustedVolume, adjustedPitch, position));
    }

    public void ChangeAudio(AudioCategory category, string key, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (generalAudioDictionary == null) return;

        AudioClipReference reference = generalAudioDictionary.GetClipReferenceByCategory(category, key);
        if (reference == null) return;

        ChangeAudio(category, reference, fadeOutTime, fadeInTime, position);
    }

    public void ChangeAudio(AudioCategory category, AudioClipReference reference, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (reference == null) return;

        if (reference.HasAudioClipSO)
        {
            ChangeAudio(category, reference.ClipSO, fadeOutTime, fadeInTime, position);
        }
        else
        {
            ChangeAudio(category, reference.Clip, fadeOutTime, fadeInTime, 1, 1, position);
        }
    }
    #endregion

    #region Private Methods
    private void SetupAudioSource(ref AudioSource source, string sourceName, AudioMixerGroup group)
    {
        if (source != null) return;

        Transform found = transform.Find(sourceName);
        if (!found)
        {
            GameObject go = new GameObject(sourceName);
            go.transform.SetParent(transform);
            source = go.AddComponent<AudioSource>();
        }
        else
        {
            source = found.GetComponent<AudioSource>();
        }

        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
    }

    private AudioSource GetSourceByCategory(AudioCategory category)
    {
        switch (category)
        {
            case AudioCategory.Background: return backgroundAudioSource;
            case AudioCategory.Music: return musicAudioSource;
            case AudioCategory.Dialogue: return dialogueAudioSource;
            default: return null; // SFX no usa un AudioSource dedicado
        }
    }

    private void PlayAudioByCategory(AudioCategory category, AudioClip clip, float clipVolume, float clipPitch,
                                   float fadeTime, Vector3 position)
    {
        if (!volumeSettings) return;

        VolumeControl volumeControl = volumeSettings.GetVolumeControlByCategory(category);
        float finalVolume = volumeControl.Volume * clipVolume;

        switch (category)
        {
            case AudioCategory.Background:
            case AudioCategory.Music:
                // música en loop
                PlayAudioInternal(clip, GetSourceByCategory(category), volumeControl, true, finalVolume, clipPitch, fadeTime);
                break;

            case AudioCategory.Dialogue:
                // Diálogo sin loop
                PlayAudioInternal(clip, dialogueAudioSource, volumeControl, false, finalVolume, clipPitch, fadeTime);
                break;

            case AudioCategory.SFX:
                // SFX en 3D a la posición dada
                Vector3 pos = position == default ? Camera.main.transform.position : position;
                PlaySoundAtPoint(clip, pos, finalVolume, clipPitch, volumeControl.Group, fadeTime);

                break;
        }
    }

    private void PlayAudioInternal(AudioClip clip, AudioSource source, VolumeControl volumeControl,
                       bool loop, float overrideVolume, float pitch, float fadeTime)
    {
        if (!clip || !source) return;

        source.clip = clip;
        source.loop = loop;
        source.pitch = pitch;

        if (fadeTime > 0f)
        {
            // Iniciamos en volumen 0, luego fade hasta overrideVolume
            source.volume = 0f;
            source.Play();
            AudioFadeUtility.FadeTo(this, source, fadeTime, overrideVolume);
        }
        else
        {
            // Sin fade
            source.volume = overrideVolume;
            source.Play();
        }
    }

    private IEnumerator ChangeAudioInternal(AudioCategory category, AudioClip clip, float clipVolume, float clipPitch, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        AudioSource source = GetSourceByCategory(category);
        if (source == null)
            yield break;

        VolumeControl volumeControl = volumeSettings.GetVolumeControlByCategory(category);

        // Fade out the currently playing audio if necessary.
        if (source.isPlaying && fadeOutTime > 0f)
        {
            bool fadeOutComplete = false;
            AudioFadeUtility.FadeOut(this, source, fadeOutTime, () => { fadeOutComplete = true; });
            // Wait until fade-out completes.
            yield return new WaitUntil(() => fadeOutComplete);
        }
        else
        {
            // If no fade-out is specified or nothing is playing, just stop immediately.
            source.Stop();
        }

        // Set up the new clip.
        source.clip = clip;
        // Optionally, configure looping based on category.
        source.loop = (category == AudioCategory.Background || category == AudioCategory.Music);
        source.pitch = clipPitch; // Adjust if needed.
        source.volume = 0f; // Start at 0 for fade-in.
        source.Play();

        // Fade in the new clip.
        if (fadeInTime > 0f)
        {
            bool fadeInComplete = false;
            AudioFadeUtility.FadeIn(this, source, fadeInTime, clipVolume, () => { fadeInComplete = true; });
            // Wait until fade-in completes.
            yield return new WaitUntil(() => fadeInComplete);

        }
        else
        {
            source.volume = clipVolume;
        }
    }
    #endregion
}

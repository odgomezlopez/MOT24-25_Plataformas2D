using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioCategory
{
    Background,
    Music,
    Dialogue,
    SFX
}

public enum AudioType
{
    OneSource,
    MultipleSource
}


[System.Serializable]
public class AudioChannelManager
{
    AudioManager audioManager;
    AudioSource audioSource;

    [SerializeField] AudioCategory category;
    [SerializeField] AudioType type;
    [SerializeField] AudioMixerGroup audioMixerGroup;
    [SerializeField] bool loop;

    public AudioChannelManager(AudioManager audioManager, AudioCategory category, AudioType type, AudioMixerGroup audioMixerGroup, bool loop)
    {
        Init(audioManager,category,type, audioMixerGroup, loop);
    }

    public void Init(AudioManager audioManager, AudioCategory category, AudioType type, AudioMixerGroup audioMixerGroup, bool loop)
    {
        this.audioManager = audioManager;
        this.category = category;
        this.type = type;
        this.audioMixerGroup = audioMixerGroup;
        this.loop = loop;

        if (this.type == AudioType.OneSource) SetupAudioSource(ref audioSource, $"{this.category.ToString()}AudioSource", audioMixerGroup);
        else
        {
            //TODO AudioPool?
            // Note: SFX uses PlaySoundAtPoint (no dedicated AudioSource)
        }
    }

    private void SetupAudioSource(ref AudioSource source, string sourceName, AudioMixerGroup group)
    {
        if (source != null) return;

        Transform found = audioManager.transform.Find(sourceName);
        if (!found)
        {
            GameObject go = new GameObject(sourceName);
            go.transform.SetParent(audioManager.transform);
            source = go.AddComponent<AudioSource>();
        }
        else
        {
            source = found.GetComponent<AudioSource>();
        }

        source.spatialBlend = 0f;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = group;
        audioSource.loop = loop;
    }


    public void PlayAudio(AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeInTime = 0f, float fadeOutTime = 0f, Vector3 position = default)
    {
        if (clip == null) return;

        if(type == AudioType.OneSource) PlayAudioInternal(clip, targetVolume, targetPitch, fadeInTime, fadeOutTime, position);
        else if (type == AudioType.MultipleSource) {
            Vector3 pos = position == default ? Camera.main.transform.position : position;
            AudioManager.PlaySoundAtPoint(clip, pos, targetVolume, targetPitch, audioMixerGroup, fadeInTime, fadeOutTime);
        }

    }

    public void ChangeAudio(AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (clip == null) return;
        audioManager.StartCoroutine(ChangeAudioInternal(clip, fadeOutTime, fadeInTime, targetVolume, targetPitch, position));
    }

    public void StopAudio(float fadeTime = 0f)
    {
        if (!audioSource || !audioSource.isPlaying) return;

        if (fadeTime > 0f)
        {
            // Fade out, luego Stop
            AudioFadeUtility.FadeOut(audioManager, audioSource, fadeTime, () => audioSource.Stop());
        }
        else
        {
            audioSource.volume = 0f;
            audioSource.Stop();
        }
    }

    public void PauseAudio(float fadeTime = 0f)
    {
        if (!audioSource || !audioSource.isPlaying)
        {
            if (category == AudioCategory.SFX) Debug.LogWarning("PauseAudio called for SFX category. Pause/resume is not supported for temporary SFX audios.");
            return;
        }

        if (fadeTime > 0f)
        {
            // Fade out, luego Pause
            AudioFadeUtility.FadeOut(audioManager, audioSource, fadeTime, () => audioSource.Pause());
        }
        else
        {
            audioSource.volume = 0f;
            audioSource.Pause();
        }
    }
    public void ResumeAudio(float fadeTime = 0f)
    {
        if (!audioSource || !audioSource.isPlaying)
        {
            if (category == AudioCategory.SFX) Debug.LogWarning("PauseAudio called for SFX category. Pause/resume is not supported for temporary SFX audios.");
            return;
        }

        // Reanuda, luego fade in
        audioSource.UnPause();

        //Obtener el valor que tenia antes de la pausa. TODO hacer DICT que almacene el valor de cada Audio 
        float toVolume = 1f;

        if (fadeTime > 0f)
        {
            AudioFadeUtility.FadeIn(audioManager, audioSource, fadeTime, toVolume);
        }
        else
        {
            audioSource.volume = toVolume;
        }
    }


    #region Private Methods
    private void PlayAudioInternal(AudioClip clip, float overrideVolume, float pitch, float fadeInTime, float fadeOutTime, Vector3 position = default)
    {
        if (!clip || !audioSource) return;

        audioSource.clip = clip;
        audioSource.pitch = pitch;

        audioSource.loop = loop;

        if (fadeInTime > 0f)
        {
            // Iniciamos en volumen 0, luego fade hasta overrideVolume
            audioSource.volume = 0f;
            audioSource.Play();
            AudioFadeUtility.FadeTo(audioManager, audioSource, fadeInTime, overrideVolume);
        }
        else
        {
            // Sin fade
            audioSource.volume = overrideVolume;
            audioSource.Play();
        }
    }

    private IEnumerator ChangeAudioInternal(AudioClip clip, float clipVolume, float clipPitch, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (audioSource == null)
            yield break;

        // Fade out the currently playing audio if necessary.
        if (audioSource.isPlaying && fadeOutTime > 0f)
        {
            bool fadeOutComplete = false;
            AudioFadeUtility.FadeOut(audioManager, audioSource, fadeOutTime, () => { fadeOutComplete = true; });
            // Wait until fade-out completes.
            yield return new WaitUntil(() => fadeOutComplete);
        }
        else
        {
            // If no fade-out is specified or nothing is playing, just stop immediately.
            audioSource.Stop();
        }

        // Set up the new clip.
        audioSource.clip = clip;
        // Optionally, configure looping based on category.
        audioSource.loop = loop;
        audioSource.pitch = clipPitch; // Adjust if needed.
        audioSource.volume = 0f; // Start at 0 for fade-in.
        audioSource.Play();

        // Fade in the new clip.
        if (fadeInTime > 0f)
        {
            bool fadeInComplete = false;
            AudioFadeUtility.FadeIn(audioManager, audioSource, fadeInTime, clipVolume, () => { fadeInComplete = true; });
            // Wait until fade-in completes.
            yield return new WaitUntil(() => fadeInComplete);

        }
        else
        {
            audioSource.volume = clipVolume;
        }
    }
    #endregion

    #region Play and Change Compability Methods
    public void PlayAudio(AudioClipSO clipSO, float fadeInTime = 0f, float fadeOutTime = 0f,  Vector3 position = default)
    {
        if (clipSO == null) return;

        AudioClip clip = clipSO.GetRandomClip();
        if (!clip) return;

        float adjustedVolume = clipSO.GetAdjustedVolume();
        float adjustedPitch = clipSO.GetAdjustedPitch();
        PlayAudio(clip, adjustedVolume, adjustedPitch, fadeInTime, fadeOutTime, position);
    }

    public void PlayAudio(AudioClipReference reference, float fadeInTime = 0f, float fadeOutTime = 0f, Vector3 position = default)
    {
        if (reference == null) return;

        if (reference.HasAudioClipSO)
        {
            PlayAudio(reference.ClipSO, fadeInTime, fadeOutTime, position);
        }
        else
        {
            PlayAudio(reference.Clip, 1, 1, fadeInTime, fadeOutTime, position);
        }
    }

    public void ChangeAudio(AudioClipSO clipSO, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (clipSO == null) return;

        AudioClip clip = clipSO.GetRandomClip();
        if (!clip) return;

        float adjustedVolume = clipSO.GetAdjustedVolume();
        float adjustedPitch = clipSO.GetAdjustedPitch();

        ChangeAudio(clip, fadeOutTime, fadeInTime, adjustedVolume, adjustedPitch, position);
    }

    public void ChangeAudio(AudioClipReference reference, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (reference == null) return;

        if (reference.HasAudioClipSO)
        {
            ChangeAudio(reference.ClipSO, fadeOutTime, fadeInTime, position);
        }
        else
        {
            ChangeAudio(reference.Clip, fadeOutTime, fadeInTime, 1, 1, position);
        }
    }
    #endregion
}

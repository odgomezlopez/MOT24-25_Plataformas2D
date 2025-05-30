using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioType
{
    OneSource,
    MultipleSource
}


//TODO Add AudioPool
[System.Serializable]
public class AudioGroupManager
{
    AudioManager audioManager;
    AudioSource audioSource;

    [SerializeField] AudioCategory category;
    [SerializeField] AudioType type;
    [SerializeField] AudioMixerGroup audioMixerGroup;
    [SerializeField] bool loop;

    public AudioGroupManager(AudioManager audioManager, AudioCategory category, AudioType type, AudioMixerGroup audioMixerGroup, bool loop)
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
        /*else
        {
            //TODO AudioPool?  Note: SFX uses PlaySoundAtPoint (no dedicated AudioSource)
        }*/
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


    public AudioSource PlayAudio(AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeInTime = 0f, float fadeOutTime = 0f, Vector3 position = default)
    {
        // Same clip already playing → nothing to do.
        if (clip == null) return null;


        // No current clip or no fade requested: immediate swap.
        if (!audioSource.isPlaying || fadeOutTime <= 0f){
            if (type == AudioType.OneSource) 
                return PlayAudioInternal(clip, targetVolume, targetPitch, fadeInTime, fadeOutTime, position);
            else if (type == AudioType.MultipleSource)
            {
                Vector3 pos = position == default ? Camera.main.transform.position : position;
                return AudioManager.PlaySoundAtPoint(clip, targetVolume, targetPitch, fadeInTime, fadeOutTime, audioMixerGroup, pos);
            }
        }

        // Otherwise cross‑fade.
        if (type == AudioType.OneSource)
            audioManager.StartCoroutine(ChangeAudioInternal(clip, fadeOutTime, fadeInTime, targetVolume, targetPitch, position));
        if (type == AudioType.MultipleSource)
            Debug.LogWarning("The change audio funcionality is currently not implemented on multiple AudioSource mode");
        
        return audioSource;
    }

    public void StopAudio(float fadeTime = 0f)
    {
        if (!audioSource || !audioSource.isPlaying) return;
        if (type == AudioType.MultipleSource)
        {
            Debug.LogWarning("The stop audio funcionality is currently not implemented on multiple AudioSource mode");
            return;
        }

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
        if (!audioSource || !audioSource.isPlaying) return;
        if (type == AudioType.MultipleSource)
        {
            Debug.LogWarning("The change audio funcionality is currently not implemented on multiple AudioSource mode");
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
    private AudioSource PlayAudioInternal(AudioClip clip, float overrideVolume, float pitch, float fadeInTime, float fadeOutTime, Vector3 position = default)
    {
        if (!clip || !audioSource) return null;

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
        return audioSource;
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
}
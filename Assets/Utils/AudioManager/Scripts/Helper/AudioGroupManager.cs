using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] AudioCategory category;
    [SerializeField] AudioType type;
    [SerializeField] AudioMode mode;

    [SerializeField] AudioMixerGroup audioMixerGroup;
    [SerializeField] bool loop;

    //AudioPool
    AudioSourcePool sourcePool;
    int sourcePoolInitSize = 3;

    public AudioGroupManager(AudioManager audioManager, AudioCategory category, AudioMode audioMode, AudioType type, AudioMixerGroup audioMixerGroup, bool loop)
    {
        Init(audioManager,category, audioMode,type, audioMixerGroup, loop);
    }

    public void Init(AudioManager audioManager, AudioCategory category, AudioMode audioMode, AudioType type, AudioMixerGroup audioMixerGroup, bool loop)
    {
        this.audioManager = audioManager;
        this.category = category;
        this.type = type; //TODO Make it take it into acount for SpacialBlend
        this.audioMixerGroup = audioMixerGroup;
        this.loop = loop;
        this.mode = audioMode;


        int initialSize = (type == AudioType.OneSource) ? 1 : sourcePoolInitSize;
        sourcePool = new(audioManager, audioMixerGroup, $"{category.ToString()}AudioSource", loop, initialSize);
    }


    public AudioSource PlayAudio(AudioClip clip, float targetVolume = 1f, float targetPitch = 1f, float fadeInTime = 0f, float fadeOutTime = 0f, Vector3 position = default)
    {
        // Same clip already playing → nothing to do.
        if (clip == null) return null;

        AudioSource source = (type == AudioType.OneSource) ? sourcePool.GetFirst() : sourcePool.GetAvailable();
        Vector3 pos = position == default ? Camera.main.transform.position : position;

        // No current clip or no fade requested: immediate swap.
        if (!source.isPlaying || fadeOutTime <= 0f){
            return PlayAudioInternal(source,clip, targetVolume, targetPitch, fadeInTime, fadeOutTime, position);
        }
        else //Change
        {
            audioManager.StartCoroutine(ChangeAudioInternal(source, clip, targetVolume, targetPitch, fadeOutTime, fadeInTime,  position));
        }
        
        return source;
    }

    public void StopAudio(float fadeTime = 0f)
    {
        foreach(AudioSource s in sourcePool.ActiveSources)
        {
            if (fadeTime > 0f)
            {
                AudioFadeUtility.FadeOut(audioManager, s, fadeTime, () => s.Stop());
            }
            else
            {
                s.volume = 0f;
                s.Stop();
            }
        }
    }

    public void PauseAudio(float fadeTime = 0f)
    {
        foreach (AudioSource s in sourcePool.ActiveSources)
        {
            if (fadeTime > 0f)
            {
                AudioFadeUtility.FadeOut(audioManager, s, fadeTime, () => s.Pause());
            }
            else
            {
                s.volume = 0f;
                s.Pause();
            }
        }
    }
    public void ResumeAudio(float fadeTime = 0f)
    {
        foreach (AudioSource s in sourcePool.ActiveSources)
        {
            s.UnPause();
            //Obtener el valor que tenia antes de la pausa. TODO hacer DICT que almacene el valor de cada Audio 
            float toVolume = 1f;

            if (fadeTime > 0f)
            {
                AudioFadeUtility.FadeIn(audioManager, s, fadeTime, toVolume);
            }
            else
            {
                s.volume = toVolume;
            }
        }
    }


    #region Private Methods
    private AudioSource PlayAudioInternal(AudioSource source, AudioClip clip, float overrideVolume, float pitch, float fadeInTime, float fadeOutTime, Vector3 position = default)
    {
        if (!clip || !source) return null;

        source.clip = clip;
        source.pitch = pitch;

        source.loop = loop;

        source.volume = (fadeInTime > 0f) ? 0f : overrideVolume;
        source.Play();
        if (fadeInTime > 0f)
            AudioFadeUtility.FadeTo(audioManager, source, fadeInTime, overrideVolume);

        return source;
    }

    private IEnumerator ChangeAudioInternal(AudioSource source, AudioClip clip, float clipVolume, float clipPitch, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (source == null)
            yield break;

        // Fade out the currently playing audio if necessary.
        if (source.isPlaying && fadeOutTime > 0f)
        {
            bool fadeOutComplete = false;
            AudioFadeUtility.FadeOut(audioManager, source, fadeOutTime, () => { fadeOutComplete = true; });
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
        source.loop = loop;
        source.pitch = clipPitch; // Adjust if needed.
        source.volume = 0f; // Start at 0 for fade-in.
        source.Play();

        // Fade in the new clip.
        if (fadeInTime > 0f)
        {
            bool fadeInComplete = false;
            AudioFadeUtility.FadeIn(audioManager, source, fadeInTime, clipVolume, () => { fadeInComplete = true; });
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
using System;
using UnityEngine;

public class AudioManagerConnector : MonoBehaviour
{
    public AudioCategory category = AudioCategory.SFX;


    [Header("Default Fade Settings")]
    [Tooltip("Valor de fade in por defecto para reproducir/reanudar audio.")]
    public float defaultFadeIn = 0f;
    [Tooltip("Valor de fade out por defecto para detener/pausar o cambiar audio.")]
    public float defaultFadeOut = 0f;

    // -------------------------------------------------------------------
    //  Métodos para Audio de Fondo (Background)
    // -------------------------------------------------------------------


    public void Play(AudioClipSO audioSO)
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.PlayAudio(audioSO, fadeInTime: defaultFadeIn);
    }

    public void Play(AudioClip clip)
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.PlayAudio(clip, fadeInTime: defaultFadeIn);
    }

    /*public void PlayBackground(string key)
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.PlayAudio(AudioCategory.Background, key, fadeTime: defaultFadeIn);
    }*/

    public void Change(AudioClipSO audioSO)
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.ChangeAudio(audioSO, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void Change(AudioClip clip)
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.ChangeAudio(clip, 1, 1, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    /*public void ChangeBackground(string key)
    {

        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, key, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }*/

    public void Stop()
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.StopAudio(fadeTime: defaultFadeOut);
    }

    public void Pause()
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.PauseAudio(fadeTime: defaultFadeOut);
    }

    public void Resume()
    {
        AudioManager.Instance?.GetChannelByCategory(category)?.ResumeAudio(fadeTime: defaultFadeIn);
    }
}

using System;
using UnityEngine;

public class AudioManagerConnector : MonoBehaviour
{
    [Header("Default Fade Settings")]
    [Tooltip("Valor de fade in por defecto para reproducir/reanudar audio.")]
    public float defaultFadeIn = 0f;
    [Tooltip("Valor de fade out por defecto para detener/pausar o cambiar audio.")]
    public float defaultFadeOut = 0f;

    // -------------------------------------------------------------------
    //  Métodos para Audio de Fondo (Background)
    // -------------------------------------------------------------------


    public void PlayBackground(AudioClipSO audioSO)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, audioSO, fadeTime: defaultFadeIn);
    }

    public void PlayBackground(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, clip, fadeTime: defaultFadeIn);
    }

    public void PlayBackground(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, key, fadeTime: defaultFadeIn);
    }
    public void ChangeBackground(AudioClipSO audioSO)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, audioSO, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void ChangeBackground(AudioClip clip)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, clip, 1, 1, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void ChangeBackground(string key)
    {

        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, key, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void StopBackground()
    {
        AudioManager.Instance?.StopAudio(AudioCategory.Background, fadeTime: defaultFadeOut);
    }

    public void PauseBackground()
    {
        AudioManager.Instance?.PauseAudio(AudioCategory.Background, fadeTime: defaultFadeOut);
    }

    public void ResumeBackground()
    {
        AudioManager.Instance?.ResumeAudio(AudioCategory.Background, fadeTime: defaultFadeIn);
    }

    // -------------------------------------------------------------------
    //  Métodos para Música (Music)
    // -------------------------------------------------------------------

    public void PlayMusic(AudioClipSO audioSO)
    {

        AudioManager.Instance?.PlayAudio(AudioCategory.Music, audioSO, fadeTime: defaultFadeIn);
    }

    public void PlayMusic(AudioClip clip)
    {

        AudioManager.Instance?.PlayAudio(AudioCategory.Music, clip, fadeTime: defaultFadeIn);
    }

    public void PlayMusic(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, key, fadeTime: defaultFadeIn);
    }

    public void ChangeMusic(AudioClipSO audioSO)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, audioSO, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void ChangeMusic(AudioClip clip)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, clip, 1f, 1f, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void ChangeMusic(string key)
    {

        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, key, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void StopMusic()
    {
        AudioManager.Instance?.StopAudio(AudioCategory.Music, fadeTime: defaultFadeOut);
    }

    public void PauseMusic()
    {
        AudioManager.Instance?.PauseAudio(AudioCategory.Music, fadeTime: defaultFadeOut);
    }

    public void ResumeMusic()
    {
        AudioManager.Instance?.ResumeAudio(AudioCategory.Music, fadeTime: defaultFadeIn);
    }

    // -------------------------------------------------------------------
    //  Métodos para Diálogo (Dialogue)
    // -------------------------------------------------------------------
    public void PlayDialogue(AudioClipSO audioSO)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, audioSO, fadeTime: defaultFadeIn);
    }

    public void PlayDialogue(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, clip, fadeTime: defaultFadeIn);
    }

    public void PlayDialogue(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, key, fadeTime: defaultFadeIn);
    }

    public void ChangeDialogue(AudioClipSO audioSO)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, audioSO, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void ChangeDialogue(AudioClip clip)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, clip, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void ChangeDialogue(string key)
    {
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, key, fadeOutTime: defaultFadeOut, fadeInTime: defaultFadeIn);
    }

    public void StopDialogue()
    {
        AudioManager.Instance?.StopAudio(AudioCategory.Dialogue, fadeTime: defaultFadeOut);
    }

    public void PauseDialogue()
    {
        AudioManager.Instance?.PauseAudio(AudioCategory.Dialogue, fadeTime: defaultFadeOut);
    }

    public void ResumeDialogue()
    {
        AudioManager.Instance?.ResumeAudio(AudioCategory.Dialogue, fadeTime: defaultFadeIn);
    }

    // -------------------------------------------------------------------
    //  Métodos para Efectos de Sonido (SFX)
    // -------------------------------------------------------------------

    public void PlaySFX(AudioClipSO audioSO)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, audioSO, fadeTime: defaultFadeIn);
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, clip, fadeTime: defaultFadeIn);
    }

    public void PlaySFX(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, key, fadeTime: defaultFadeIn);
    }
}

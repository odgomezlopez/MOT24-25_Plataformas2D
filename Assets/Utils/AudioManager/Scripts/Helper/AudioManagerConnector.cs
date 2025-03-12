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
    public void PlayBackground(AudioClipReference reference, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, reference, fadeTime, position);
    }

    public void PlayBackground(AudioClipSO audioSO, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, audioSO, fadeTime, position);
    }

    public void PlayBackground(AudioClip clip, float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, clip, fadeTime);
    }

    public void PlayBackground(string key, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, key, fadeTime, position);
    }

    public void ChangeBackground(AudioClipReference reference, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, reference, fadeOutTime, fadeInTime, position);
    }

    public void ChangeBackground(AudioClipSO audioSO, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, audioSO, fadeOutTime, fadeInTime, position);
    }

    public void ChangeBackground(AudioClip clip, float targetVolume, float targetPitch, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, clip, targetVolume, targetPitch, fadeOutTime, fadeInTime, position);
    }

    public void ChangeBackground(string key, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Background, key, fadeOutTime, fadeInTime, position);
    }

    public void StopBackground(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeOut;
        AudioManager.Instance?.StopAudio(AudioCategory.Background, fadeTime);
    }

    public void PauseBackground(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeOut;
        AudioManager.Instance?.PauseAudio(AudioCategory.Background, fadeTime);
    }

    public void ResumeBackground(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.ResumeAudio(AudioCategory.Background, fadeTime);
    }

    // -------------------------------------------------------------------
    //  Métodos para Música (Music)
    // -------------------------------------------------------------------
    public void PlayMusic(AudioClipReference reference, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, reference, fadeTime, position);
    }

    public void PlayMusic(AudioClipSO audioSO, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, audioSO, fadeTime, position);
    }

    public void PlayMusic(AudioClip clip, float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, clip, fadeTime);
    }

    public void PlayMusic(string key, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, key, fadeTime, position);
    }

    public void ChangeMusic(AudioClipReference reference, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, reference, fadeOutTime, fadeInTime, position);
    }

    public void ChangeMusic(AudioClipSO audioSO, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, audioSO, fadeOutTime, fadeInTime, position);
    }

    public void ChangeMusic(AudioClip clip, float targetVolume, float targetPitch, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, clip, targetVolume, targetPitch, fadeOutTime, fadeInTime, position);
    }

    public void ChangeMusic(string key, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Music, key, fadeOutTime, fadeInTime, position);
    }

    public void StopMusic(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeOut;
        AudioManager.Instance?.StopAudio(AudioCategory.Music, fadeTime);
    }

    public void PauseMusic(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeOut;
        AudioManager.Instance?.PauseAudio(AudioCategory.Music, fadeTime);
    }

    public void ResumeMusic(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.ResumeAudio(AudioCategory.Music, fadeTime);
    }

    // -------------------------------------------------------------------
    //  Métodos para Diálogo (Dialogue)
    // -------------------------------------------------------------------
    public void PlayDialogue(AudioClipReference reference, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, reference, fadeTime, position);
    }

    public void PlayDialogue(AudioClipSO audioSO, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, audioSO, fadeTime, position);
    }

    public void PlayDialogue(AudioClip clip, float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, clip, fadeTime);
    }

    public void PlayDialogue(string key, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, key, fadeTime, position);
    }

    public void ChangeDialogue(AudioClipReference reference, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, reference, fadeOutTime, fadeInTime, position);
    }

    public void ChangeDialogue(AudioClipSO audioSO, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, audioSO, fadeOutTime, fadeInTime, position);
    }

    public void ChangeDialogue(AudioClip clip, float targetVolume, float targetPitch, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, clip, targetVolume, targetPitch, fadeOutTime, fadeInTime, position);
    }

    public void ChangeDialogue(string key, float fadeOutTime = -1f, float fadeInTime = -1f, Vector3 position = default)
    {
        if (fadeOutTime < 0f)
            fadeOutTime = defaultFadeOut;
        if (fadeInTime < 0f)
            fadeInTime = defaultFadeIn;
        AudioManager.Instance?.ChangeAudio(AudioCategory.Dialogue, key, fadeOutTime, fadeInTime, position);
    }

    public void StopDialogue(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeOut;
        AudioManager.Instance?.StopAudio(AudioCategory.Dialogue, fadeTime);
    }

    public void PauseDialogue(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeOut;
        AudioManager.Instance?.PauseAudio(AudioCategory.Dialogue, fadeTime);
    }

    public void ResumeDialogue(float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.ResumeAudio(AudioCategory.Dialogue, fadeTime);
    }

    // -------------------------------------------------------------------
    //  Métodos para Efectos de Sonido (SFX)
    // -------------------------------------------------------------------
    public void PlaySFX(AudioClipReference reference, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, reference, fadeTime, position);
    }

    public void PlaySFX(AudioClipSO audioSO, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, audioSO, fadeTime, position);
    }

    public void PlaySFX(AudioClip clip, float fadeTime = -1f)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, clip, fadeTime);
    }

    public void PlaySFX(string key, float fadeTime = -1f, Vector3 position = default)
    {
        if (fadeTime < 0f)
            fadeTime = defaultFadeIn;
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, key, fadeTime, position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


//Improvements: - SFXs using Audio Pool. - Pause and Resume SFXs
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region Fields and References

    [SerializeField, Expandable] private VolumeSettings volumeSettings;

    public AudioChannelManager backgroundManager;
    public AudioChannelManager musicManager;
    public AudioChannelManager dialogueManager;
    public AudioChannelManager sfxManager;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        // Aseguramos que cada AudioSource existe; si no, lo creamos
        //TODO Mover estos datos puestos aquí a mano a las categorias de volumeSettings y ocultar en el inspector
        backgroundManager = new AudioChannelManager(this,AudioCategory.Background,AudioType.OneSource, volumeSettings.background,true);
        musicManager = new AudioChannelManager(this, AudioCategory.Music, AudioType.OneSource, volumeSettings.music, true);
        dialogueManager = new AudioChannelManager(this, AudioCategory.Dialogue, AudioType.OneSource, volumeSettings.dialogue, false);
        sfxManager = new AudioChannelManager(this, AudioCategory.SFX, AudioType.MultipleSource, volumeSettings.sfx, false);
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
    public AudioChannelManager GetChannelByCategory(AudioCategory category)
    {
        switch (category)
        {
            case AudioCategory.Background: return backgroundManager;
            case AudioCategory.Music: return musicManager;
            case AudioCategory.Dialogue: return dialogueManager;
            case AudioCategory.SFX: return sfxManager;

            default: return null; // SFX no usa un AudioSource dedicado
        }
    }

    // Optional methods to pause/resume all non-SFX audio.
    public void PauseAllAudio(float fadeTime = 0f)
    {
        backgroundManager.PauseAudio(fadeTime);
        musicManager.PauseAudio(fadeTime);
        dialogueManager.PauseAudio(fadeTime);
        sfxManager.PauseAudio(fadeTime);

    }

    public void ResumeAllAudio(float fadeTime = 0f)
    {
        backgroundManager.ResumeAudio(fadeTime);
        musicManager.ResumeAudio(fadeTime);
        dialogueManager.ResumeAudio(fadeTime);
        sfxManager.ResumeAudio(fadeTime);
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

    //[SerializeField] private AudioDictionary generalAudioDictionary;
    //public bool HasKey(AudioCategory category, string key) => generalAudioDictionary?.HasKey(category, key) ?? false;

    /*
    public void PlayAudio(AudioCategory category, string key, float fadeTime = 0f, Vector3 position = default)
    {
        if (generalAudioDictionary == null) return;

        AudioClipReference reference = generalAudioDictionary.GetClipReferenceByCategory(category, key);
        if (reference == null) return;

        PlayAudio(category, reference, fadeTime: fadeTime, position);
    }

    public void ChangeAudio(AudioCategory category, string key, float fadeOutTime = 0f, float fadeInTime = 0f, Vector3 position = default)
    {
        if (generalAudioDictionary == null) return;

        AudioClipReference reference = generalAudioDictionary.GetClipReferenceByCategory(category, key);
        if (reference == null) return;

        ChangeAudio(category, reference, fadeOutTime, fadeInTime, position);
    }

     */





}

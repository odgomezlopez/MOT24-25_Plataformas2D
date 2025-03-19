using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.Rendering.PostProcessing.HistogramMonitor;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region Fields and References

    [SerializeField, Expandable] private VolumeSettings volumeSettings;

    private Dictionary<AudioCategory,AudioChannelManager> audioChannels;

    //[SerializeField] private AudioDictionary globalAudios;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        // Aseguramos que cada AudioSource existe; si no, lo creamos
        //TODO Mover estos datos puestos aqu� a mano a las categorias de volumeSettings y ocultar en el inspector
        audioChannels = new();
        foreach (KeyValuePair<AudioCategory, VolumeChannelControl> v in volumeSettings.channels)
            audioChannels.Add(v.Key,new AudioChannelManager(this,v.Key, v.Value.Type, v.Value.Group,v.Value.loop));
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
        return audioChannels.GetValueOrDefault(category);
    }

    // Optional methods to pause/resume all non-SFX audio.
    public void PauseAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioChannelManager> entry in audioChannels)
        {
            entry.Value.PauseAudio(fadeTime);
        }
    }

    public void ResumeAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioChannelManager> entry in audioChannels)
        {
            entry.Value.ResumeAudio(fadeTime);
        }
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

            // Do not call tempSource.Play() here�the AudioFade's Start() method will handle it.
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

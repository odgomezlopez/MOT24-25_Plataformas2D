using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    #region Fields and References

    [SerializeField, Expandable] private VolumeSettings volumeSettings;
    private Dictionary<AudioCategory,AudioGroupManager> audioGroups;

    #endregion

    #region Unity Lifecycle

    private void Awake()
    {
        //Creamos un AudioGroupManager por cada VolumeSetting
        audioGroups = new();
        foreach (KeyValuePair<AudioCategory, VolumeGroupControl> v in volumeSettings.groups)
            audioGroups.Add(v.Key,new AudioGroupManager(this, v.Key, v.Value.audioMode, v.Value.Type, v.Value.Group,v.Value.loop));
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
    public AudioGroupManager GetChannelByCategory(AudioCategory category)
    {
        return audioGroups.GetValueOrDefault(category);
    }

    // Optional methods to pause/resume all non-SFX audio.
    public void PauseAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioGroupManager> entry in audioGroups)
        {
            entry.Value.PauseAudio(fadeTime);
        }
    }

    public void ResumeAllAudio(float fadeTime = 0f)
    {
        foreach (KeyValuePair<AudioCategory, AudioGroupManager> entry in audioGroups)
        {
            entry.Value.ResumeAudio(fadeTime);
        }
    }

    /// <summary>
    /// Audio 3D one-shot sin un AudioSource persistente.
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

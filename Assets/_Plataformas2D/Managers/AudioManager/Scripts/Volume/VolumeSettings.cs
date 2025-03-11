using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class VolumeControl
{
    [SerializeField, Range(0, 1)] private float volume = 1f;
    [SerializeField] private string parameter = "MasterVolume";
    [SerializeField] private AudioMixerGroup group;

    public float Volume
    {
        get => volume;
        set => SetVolume(value);
    }

    public string ParameterName => parameter;
    public AudioMixerGroup Group => group;

    private void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);

        if (group?.audioMixer == null || string.IsNullOrEmpty(parameter))
        {
            Debug.LogWarning($"VolumeControl: Invalid mixer/parameter: {parameter}");
            return;
        }

        float decibels = LinearToDecibel(volume);
        group.audioMixer.SetFloat(parameter, decibels);
    }

    private static float LinearToDecibel(float linear, float minDecibels = -80f)
    {
        return (linear > 0.0001f) ? 20f * Mathf.Log10(linear) : minDecibels;
    }
}


[CreateAssetMenu(fileName = "VolumeSettings", menuName = "Settings/VolumeSettings", order = 1)]
public class VolumeSettings : ScriptableObject
{
    public VolumeControl master;
    public VolumeControl background;
    public VolumeControl music;
    public VolumeControl dialogue;
    public VolumeControl sfx;

    public void LoadVolumeSettings()
    {
        master.Volume = PlayerPrefs.GetFloat(master.ParameterName, 1f);
        background.Volume = PlayerPrefs.GetFloat(background.ParameterName, 1f);
        music.Volume = PlayerPrefs.GetFloat(music.ParameterName, 1f);
        dialogue.Volume = PlayerPrefs.GetFloat(dialogue.ParameterName, 1f);
        sfx.Volume = PlayerPrefs.GetFloat(sfx.ParameterName, 1f);
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(master.ParameterName, master.Volume);
        PlayerPrefs.SetFloat(background.ParameterName, background.Volume);
        PlayerPrefs.SetFloat(music.ParameterName, music.Volume);
        PlayerPrefs.SetFloat(dialogue.ParameterName, dialogue.Volume);
        PlayerPrefs.SetFloat(sfx.ParameterName, sfx.Volume);
    }

    public void ResetVolumeSettings()
    {
        master.Volume = 1f;
        background.Volume = 1f;
        music.Volume = 1f;
        dialogue.Volume = 1f;
        sfx.Volume = 1f;

        SaveVolumeSettings();
    }

    private void OnValidate()
    {
        // This forces volumes to be reapplied in the editor
        master.Volume = master.Volume;
        background.Volume = background.Volume;
        music.Volume = music.Volume;
        dialogue.Volume = dialogue.Volume;
        sfx.Volume = sfx.Volume;

        SaveVolumeSettings();
    }
}

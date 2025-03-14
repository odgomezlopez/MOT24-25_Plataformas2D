using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

[System.Serializable]
public class VolumeControl
{
    [SerializeField] public AudioCategory Category;
    [SerializeField] private AudioMixerGroup group;

    [SerializeField] public AudioType Type;

    [SerializeField, Range(0, 1)] private float volume = 1f;
    [SerializeField] public bool loop;


    public float Volume
    {
        get => volume;
        set => SetVolume(value);
    }

    public string ParameterName => $"{group.name}Volume";
    public AudioMixerGroup Group => group;

    private void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        //group.name;
        if (group?.audioMixer == null || string.IsNullOrEmpty(ParameterName))
        {
            Debug.LogWarning($"VolumeControl: Invalid mixer/parameter: {ParameterName}");
            return;
        }

        float decibels = LinearToDecibel(volume);
        group.audioMixer.SetFloat(ParameterName, decibels);
    }

    private static float LinearToDecibel(float linear, float maxDecibels = 0f, float minDecibels = -80f)
    {
        return (linear > 0.0001f) ? 20f * Mathf.Log10(linear) : minDecibels;
    }
}


[CreateAssetMenu(fileName = "VolumeSettings", menuName = "Settings/VolumeSettings", order = 1)]
public class VolumeSettings : ScriptableObject
{
    public VolumeControl master;
    public List<VolumeControl> channels;
    
    /// <summary>
    /// Devuelve el VolumeControl adecuado seg�n la categor�a.
    /// </summary>
    public VolumeControl GetVolumeControlByCategory(AudioCategory category)
    {
        switch (category)
        {
            case AudioCategory.Background: return channels[0];
            case AudioCategory.Music: return channels[1];
            case AudioCategory.Dialogue: return channels[2];
            case AudioCategory.SFX: return channels[3];
            default: return master; // fallback
        }
    }

    public void LoadVolumeSettings()
    {
        master.Volume = PlayerPrefs.GetFloat(master.ParameterName, 1f);
        foreach(VolumeControl v in channels)
            v.Volume = PlayerPrefs.GetFloat(v.ParameterName, 1f);
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(master.ParameterName, master.Volume);
        master.Volume = PlayerPrefs.GetFloat(master.ParameterName, 1f);
        foreach (VolumeControl v in channels)
            PlayerPrefs.SetFloat(v.ParameterName, v.Volume);
    }

    public void ResetVolumeSettings()
    {
        master.Volume = 1f;
        foreach (VolumeControl v in channels)
            v.Volume = 1f;

        SaveVolumeSettings();
    }

    private void OnValidate()
    {
        // This forces volumes to be reapplied in the editor
        master.Volume = master.Volume;
        foreach (VolumeControl v in channels)
            v.Volume = v.Volume;

        SaveVolumeSettings();
    }
}

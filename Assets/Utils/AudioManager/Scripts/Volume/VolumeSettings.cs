using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public enum AudioMode
{
    Audio2D,
    Audio3D
}

[System.Serializable]
public class VolumeControl
{
    [SerializeField] private AudioMixerGroup group;
    [SerializeField, Range(0, 1)] private float volume = 1f;

    [SerializeField, Range(VolumeMathUtility.MIN_DECIBELS, VolumeMathUtility.MAX_DECIBELS)] private float localMaxDB = VolumeMathUtility.MAX_DECIBELS; 

    //Getter y Setters
    public float Volume { get => volume; set => SetVolume(value); }
    public float LocalMaxDB { get => localMaxDB; set => localMaxDB = value; }

    public string ParameterName => $"{group.name}Volume";
    public AudioMixerGroup Group => group;
    public AudioMode audioMode = AudioMode.Audio2D;

    private void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        //group.name;
        if (group?.audioMixer == null || string.IsNullOrEmpty(ParameterName))
        {
            Debug.LogWarning($"VolumeControl: Invalid mixer/parameter. Please expose the parameter of the AudioGroupMix as {group.name}Volume");
            return;
        }

        float decibels = VolumeMathUtility.MapVolumeToDecibels(volume, maxDb: localMaxDB);
        group.audioMixer.SetFloat(ParameterName, decibels);
    }
}

[System.Serializable]
public class VolumeGroupControl : VolumeControl
{
    [SerializeField] public AudioType Type;
    [SerializeField] public bool loop;
}


[CreateAssetMenu(fileName = "VolumeSettings", menuName = "Settings/VolumeSettings", order = 1)]
public class VolumeSettings : ScriptableObject
{
    public VolumeControl master;
    [SerializeField] public SerializedDictionary<AudioCategory, VolumeGroupControl> groups; //Requiere el uso de la siguiente dependencia: https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052


    /// <summary>
    /// Devuelve el VolumeControl adecuado según la categoría.
    /// </summary>
    public VolumeGroupControl GetVolumeControlByCategory(AudioCategory category)
    {
        VolumeGroupControl v;
        if (groups.TryGetValue(category, out v)) return v;
        else return null;
    }

    public void LoadVolumeSettings()
    {
        master.Volume = PlayerPrefs.GetFloat(master.ParameterName, 1f);

        foreach(KeyValuePair<AudioCategory,VolumeGroupControl> v in groups)
        {
            v.Value.Volume = PlayerPrefs.GetFloat(v.Value.ParameterName, 1f);
            v.Value.LocalMaxDB = PlayerPrefs.GetFloat(v.Value.ParameterName + "MaxDB", 0f);
        }
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(master.ParameterName, master.Volume);
        master.Volume = PlayerPrefs.GetFloat(master.ParameterName, 1f);

        foreach (KeyValuePair<AudioCategory, VolumeGroupControl> v in groups)
        {
            PlayerPrefs.SetFloat(v.Value.ParameterName, v.Value.Volume);
            PlayerPrefs.SetFloat(v.Value.ParameterName + "MaxDB", v.Value.LocalMaxDB);
        }
        PlayerPrefs.Save();
    }

    public void ResetVolumeSettings()
    {
        master.Volume = 1f;
        foreach (KeyValuePair<AudioCategory, VolumeGroupControl> v in groups)
            v.Value.Volume = 1f;

        SaveVolumeSettings();
    }

    private void OnValidate()
    {
        // This forces volumes to be reapplied in the editor
        master.Volume = master.Volume;
        foreach (KeyValuePair<AudioCategory, VolumeGroupControl> v in groups)
        {
            v.Value.Volume = v.Value.Volume;
            v.Value.LocalMaxDB = v.Value.LocalMaxDB;

        }

        SaveVolumeSettings();
    }
}

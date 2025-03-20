using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class AudioClipReference
{
    [Tooltip("Check to use an AudioClipSO. Otherwise an AudioClip is used.")]
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClipSO audioClipSO;

    public AudioClip Clip => audioClip;
    public bool HasAudioClipSO => audioClipSO != null;
    public AudioClipSO ClipSO => audioClipSO;
}

[System.Serializable]
public class AudioDictionary
{
    //Requiere el uso de la siguiente dependencia: https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052
    [SerializeField] private AudioCategory audioCategory = AudioCategory.Music;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> clipsDict;

    public AudioCategory AudioCategory { get => audioCategory; set => audioCategory = value; }
    //Constructor
    public AudioDictionary(AudioCategory audioCategory)
    {
        this.audioCategory = audioCategory;
    }

    public bool HasKey(string key)
    {
        return clipsDict.ContainsKey(key);
    }

    /// <summary>
    /// Recupera el AudioClipReference desde el AudioDictionary para la key dada.
    /// </summary>
    public AudioClipReference GetClipReference(string key)
    {
        if (clipsDict != null && clipsDict.TryGetValue(key, out var reference))
            return reference;
        return null;
    }


    //[SerializeField] private AudioDictionary generalAudioDictionary;
    public void PlayAudio(string key, Vector3 position = default)
    {

        AudioClipReference clipRef = GetClipReference(key);
        if (clipRef != null) AudioManager.Instance.GetChannelByCategory(audioCategory).PlayAudio(clipRef);
    }

    public void ChangeAudio(string key, Vector3 position = default)
    {
        AudioClipReference clipRef = GetClipReference(key);
        if (clipRef != null) AudioManager.Instance.GetChannelByCategory(audioCategory).ChangeAudio(clipRef);
    }

}


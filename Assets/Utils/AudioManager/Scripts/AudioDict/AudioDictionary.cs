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
    [Header("AudioClip Dictionaries")]
    [SerializeField] private SerializedDictionary<string, AudioClipReference> backgroundClips;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> musicClips;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> sfxClips;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> dialogueClips;


    public bool HasKey(AudioCategory category, string key)
    {
        switch (category)
        {
            case AudioCategory.Background: return GetBackgroundClipReference(key) != null;
            case AudioCategory.Music: return GetMusicClipReference(key) != null;
            case AudioCategory.Dialogue: return GetDialogueClipReference(key) != null;
            case AudioCategory.SFX: return GetSfxClipReference(key) != null;
            default: return false;
        }
    }

    /// <summary>
    /// Recupera el AudioClipReference desde el AudioDictionary para la categoría y key dada.
    /// </summary>
    public AudioClipReference GetClipReferenceByCategory(AudioCategory category, string key)
    {
        switch (category)
        {
            case AudioCategory.Background: return GetBackgroundClipReference(key);
            case AudioCategory.Music: return GetMusicClipReference(key);
            case AudioCategory.Dialogue: return GetDialogueClipReference(key);
            case AudioCategory.SFX: return GetSfxClipReference(key);
            default: return null;
        }
    }


    /// <summary>
    /// Returns the <see cref="AudioClipReference"/> for Background audio matching the given key.
    /// </summary>
    public AudioClipReference GetBackgroundClipReference(string key)
    {
        if (backgroundClips != null && backgroundClips.TryGetValue(key, out var reference))
            return reference;
        return null;
    }

    /// <summary>
    /// Returns the <see cref="AudioClipReference"/> for Music audio matching the given key.
    /// </summary>
    public AudioClipReference GetMusicClipReference(string key)
    {
        if (musicClips != null && musicClips.TryGetValue(key, out var reference))
            return reference;
        return null;
    }

    /// <summary>
    /// Returns the <see cref="AudioClipReference"/> for SFX audio matching the given key.
    /// </summary>
    public AudioClipReference GetSfxClipReference(string key)
    {
        if (sfxClips != null && sfxClips.TryGetValue(key, out var reference))
            return reference;
        return null;
    }

    /// <summary>
    /// Returns the <see cref="AudioClipReference"/> for Dialogue audio matching the given key.
    /// </summary>
    public AudioClipReference GetDialogueClipReference(string key)
    {
        if (dialogueClips != null && dialogueClips.TryGetValue(key, out var reference))
            return reference;
        return null;
    }

    //[SerializeField] private AudioDictionary generalAudioDictionary;
    public void PlayAudio(AudioCategory category, string key, Vector3 position = default)
    {

        AudioClipReference clipRef = GetClipReferenceByCategory(category, key);
        if (clipRef != null) AudioManager.Instance.GetChannelByCategory(category).PlayAudio(clipRef);
    }

    public void ChangeAudio(AudioCategory category, string key, Vector3 position = default)
    {
        AudioClipReference clipRef = GetClipReferenceByCategory(category, key);
        if (clipRef != null) AudioManager.Instance.GetChannelByCategory(category).ChangeAudio(clipRef);
    }

}


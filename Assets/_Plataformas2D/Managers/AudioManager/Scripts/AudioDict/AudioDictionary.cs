using UnityEngine;
using AYellowpaper.SerializedCollections;

[System.Serializable]
public class AudioDictionary
{
    [Header("AudioClip Dictionaries")]
    [SerializeField] private SerializedDictionary<string, AudioClipReference> backgroundClips;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> musicClips;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> sfxClips;
    [SerializeField] private SerializedDictionary<string, AudioClipReference> dialogueClips;

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
}

using UnityEngine;
using AYellowpaper.SerializedCollections;


[CreateAssetMenu(fileName = "AudioDictSO", menuName = "AudioSO/Audio Dict")]
public class AudioDictionarySO : ScriptableObject
{
    //Requiere el uso de la siguiente dependencia: https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052
    [SerializeField] private SerializedDictionary<string, AudioClipSO> clipsDict;

    public bool HasKey(string key)
    {
        return clipsDict.ContainsKey(key);
    }

    /// <summary>
    /// Recupera el AudioClipReference desde el AudioDictionary para la key dada.
    /// </summary>
    public AudioClipSO GetClip(string key)
    {
        if (clipsDict != null && clipsDict.TryGetValue(key, out var reference))
            return reference;
        return null;
    }


    //[SerializeField] private AudioDictionary generalAudioDictionary;
    public void PlayAudio(string key, Vector3 position = default)
    {
        AudioClipSO clipRef = GetClip(key);

        if (clipRef != null) clipRef.Play(position);
    }
}


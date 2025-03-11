using UnityEngine;

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
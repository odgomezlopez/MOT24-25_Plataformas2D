using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This component plays default background and music audio on Start.
/// If an AudioClipReference is provided, it uses that. Otherwise, it falls back to the string key.
/// </summary>
public class AudioOnStart : MonoBehaviour
{
    [Header("Default audio")]
    [SerializeField] private List<AudioClipSO> audioClips;
     
    // Start is called before the first frame update
    private void Start()
    {
        foreach(AudioClipSO a in audioClips)
            a.Play();
    }
}

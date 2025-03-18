using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This component plays default background and music audio on Start.
/// If an AudioClipReference is provided, it uses that. Otherwise, it falls back to the string key.
/// </summary>
public class AudioStart : MonoBehaviour
{
    [Header("Default audio")]
    [SerializeField] private AudioCategory category = AudioCategory.Music;
    [SerializeField] private AudioClipReference reference;
    //[SerializeField] private string key="";
     
    // Start is called before the first frame update
    private void Start()
    {
        // Background Init
        //if(key != "" && AudioManager.Instance.HasKey(key))
        //   AudioManager.Instance.backgroundManager.PlayAudio(backgroundKey, fadeTime: backgroundFadeIn);
        //else

        AudioManager.Instance.GetChannelByCategory(category).PlayAudio(reference);
    }
}

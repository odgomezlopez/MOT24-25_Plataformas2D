using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// This component plays default background and music audio on Start.
/// If an AudioClipReference is provided, it uses that. Otherwise, it falls back to the string key.
/// </summary>
public class AudioDefault : MonoBehaviour
{
    [Header("Background default audio")]
    [SerializeField] private AudioClipReference backgroundReference;
    [SerializeField] private string backgroundKey="";

    [SerializeField] private float backgroundFadeIn = 1f;

    [Header("Music default audio")]
    [SerializeField] private AudioClipReference musicReference;
    [SerializeField] private string musicKey="";

    [SerializeField] private float musicFadeIn = 1f;

    // Start is called before the first frame update
    private void Start()
    {
        // Background Init
        if(backgroundKey != "" && AudioManager.Instance.HasKey(AudioCategory.Background,backgroundKey))
           AudioManager.Instance.PlayAudio(AudioCategory.Background, backgroundKey, fadeTime: backgroundFadeIn);
        else
            AudioManager.Instance.PlayAudio(AudioCategory.Background, backgroundReference, fadeTime: backgroundFadeIn);

        // Music Init
        if (musicKey != "" && AudioManager.Instance.HasKey(AudioCategory.Music, musicKey))
            AudioManager.Instance.PlayAudio(AudioCategory.Music, musicKey, fadeTime: musicFadeIn);
        else
            AudioManager.Instance.PlayAudio(AudioCategory.Music, musicReference, fadeTime: musicFadeIn);
    }
}

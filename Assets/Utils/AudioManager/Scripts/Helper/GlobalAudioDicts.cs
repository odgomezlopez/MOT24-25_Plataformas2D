using UnityEngine;

public class GlobalAudioDicts : MonoBehaviourSingleton<GlobalAudioDicts>
{
    [SerializeField, Expandable] public AudioDictionarySO background;
    [SerializeField, Expandable] public AudioDictionarySO music;
    [SerializeField, Expandable] public AudioDictionarySO dialogue;
    [SerializeField, Expandable] public AudioDictionarySO sfx;
    [SerializeField, Expandable] public AudioDictionarySO ui;

    public AudioDictionarySO GetAudioDictionaryByCategory(AudioCategory audioCategory)
    {
        switch (audioCategory)
        {
            case AudioCategory.Background: return background;
            case AudioCategory.Music: return music;
            case AudioCategory.Dialogue: return dialogue;
            case AudioCategory.SFX: return sfx;
            case AudioCategory.UI: return ui;
            default: return null;

        }
    }
}

using UnityEngine;

public class AudioManagerConnector : MonoBehaviour
{
    // -------------------------------------------------------------------
    //  Background methods
    // -------------------------------------------------------------------
    public void PlayBackground(AudioClipReference reference)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, reference);
    }

    public void PlayBackground(AudioClipSO audioSO)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, audioSO);
    }

    public void PlayBackground(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, clip);
    }

    public void PlayBackground(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Background, key);
    }

    // -------------------------------------------------------------------
    //  Music methods
    // -------------------------------------------------------------------
    public void PlayMusic(AudioClipReference reference)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, reference);
    }

    public void PlayMusicByKey(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, key);
    }

    public void PlayMusicClip(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Music, clip);
    }

    // -------------------------------------------------------------------
    //  Dialogue methods
    // -------------------------------------------------------------------
    public void PlayDialogue(AudioClipReference reference)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, reference);
    }

    public void PlayDialogueByKey(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, key);
    }

    public void PlayDialogueClip(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.Dialogue, clip);
    }

    // -------------------------------------------------------------------
    //  SFX methods
    // -------------------------------------------------------------------
    public void PlaySFX(AudioClipReference reference)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, reference);
    }

    public void PlaySFXByKey(string key)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, key);
    }

    public void PlaySFXClip(AudioClip clip)
    {
        AudioManager.Instance?.PlayAudio(AudioCategory.SFX, clip);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class CollectCoin : MonoBehaviour
{
    [Header("Score")]
    [SerializeField] int scoreAdd = 1;

    [Header("Events")]
    [SerializeField] UnityEvent OnTrigger;
    [SerializeField] UnityEvent<int> OnScore;

    [Header("Extra")]
    [SerializeField] AudioClipReference audioClip;

    bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject.CompareTag("Player"))
        {
            triggered = true;

            OnTrigger.Invoke();
            //OnScore.Invoke(scoreAdd);
            ScoreManager.Instance.AddScore(scoreAdd);


            AudioManager.Instance.GetChannelByCategory(AudioCategory.SFX).PlayAudio(audioClip);
            
            Destroy(gameObject);
        }
    }

    private void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume = 1, float pitch = 1)
    {
        if (clip == null) return;

        var tempGO = new GameObject("TempAudio") { transform = { position = position } };
        var audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = clip;

        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.Play();
        Destroy(tempGO, clip.length / Mathf.Abs(pitch));
    }
}

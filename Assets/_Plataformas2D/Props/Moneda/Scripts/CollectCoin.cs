using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CollectCoin : MonoBehaviour
{
    [SerializeField] AudioClip audioClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (audioClip)
            {
                //AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, 1);//Opción 1. Utilizando AudioSource
                PlaySoundAtPoint(audioClip,Camera.main.transform.position);
            }
            
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

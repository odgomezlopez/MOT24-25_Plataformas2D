using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenEnd : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, audioSource.clip.length);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    //Versión 1. Utilizando el instanciador de AudioSource, pero falla un poco en el efecto Doppler
    [SerializeField] AudioClip audioClip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (audioClip) AudioSource.PlayClipAtPoint(audioClip,Camera.main.transform.position,1);
            Destroy(gameObject);
        }
    }

    //Versión 2. Instanciando Prefabs con AudioSource con PlayOnAwake y el Script DestroyWhenEnd
    /*[SerializeField] GameObject coinSFX;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(coinSFX) Instantiate(coinSFX,null);
            Destroy(gameObject);
        }
    }*/
}

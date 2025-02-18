using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class OnCollisionPlayerAsChild2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtén el transform del objeto raíz (o del padre si el player tiene hijos)
            Transform playerRoot = collision.GetComponentInParent<ActorController>().transform;

            // Establece el nuevo padre como este objeto
            playerRoot.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtén el transform del objeto raíz (o del padre si el player tiene hijos)
            Transform playerRoot = collision.GetComponentInParent<ActorController>().transform;

            // Quitar padre
            playerRoot.SetParent(null);
  
        }
    }
}

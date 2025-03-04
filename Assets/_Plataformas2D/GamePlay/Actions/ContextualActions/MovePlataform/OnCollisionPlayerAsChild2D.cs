using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class OnCollisionPlayerAsChild2D : MonoBehaviour
{
    [SerializeField] private string[] actorTags = { "Player" };

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsValidActorTag(collision.tag))
        {
            // Obtén el transform del objeto raíz (o del padre si el player tiene hijos)
            Transform playerRoot = collision.GetComponentInParent<ActorController>().transform;

            // Establece el nuevo padre como este objeto
            playerRoot.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsValidActorTag(collision.tag))
        {
            // Obtén el transform del objeto raíz (o del padre si el player tiene hijos)
            Transform playerRoot = collision.GetComponentInParent<ActorController>().transform;

            // Quitar padre
            playerRoot.SetParent(null);
  
        }
    }

    private bool IsValidActorTag(string tag)
    {
        foreach (var validTag in actorTags)
        {
            if (tag.Equals(validTag))
                return true;
        }
        return false;
    }
}

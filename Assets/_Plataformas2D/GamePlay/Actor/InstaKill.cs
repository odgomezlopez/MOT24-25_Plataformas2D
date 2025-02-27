using UnityEngine;
using static UnityEngine.Rendering.STP;
using static UnityEngine.UI.Image;

public class InstaKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var actor = collision.GetComponentInParent<ActorController>();
        if(actor)
            actor.Die();
    }
}

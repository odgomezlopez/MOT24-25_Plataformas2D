using UnityEngine;
using static UnityEngine.Rendering.STP;
using static UnityEngine.UI.Image;

public class InstaKill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collisions with actor controllers.
        if (collision.TryGetComponent(out ActorController actor))
            actor.Die();
    }
}

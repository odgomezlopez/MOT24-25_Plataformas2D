using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] public float damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        IActorController actor = collision.GetComponent<IActorController>();
        if(actor != null)
        {
            actor.TakeDamage(damage, gameObject);
        }
    }
}

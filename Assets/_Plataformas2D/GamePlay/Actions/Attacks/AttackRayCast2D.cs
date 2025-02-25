using UnityEngine;

[CreateAssetMenu(fileName = "new AttackRayCast", menuName = "Actions/Attack/RayCast", order = 1)]

public class AttackRayCast2D : Attack
{
    [Header("Raycast")]
    public float rayDistance = 4f;

    //Sobreescribimos el método use
    public override void Use(GameObject g)
    {
        base.Use(g);
        FlipSprite2D flipSprite2D = g.GetComponent<FlipSprite2D>();

        Vector2 dir = (flipSprite2D.IsFacingRight) ? Vector2.right : Vector2.left;
        var hit = Physics2D.Raycast(g.transform.position, dir, rayDistance); //TODO Review the layer system.


        if (hit.collider != null)
        {
            ActorController actor = hit.collider.gameObject.GetComponentInParent<ActorController>();
            if (actor) actor.TakeDamage(damage,g);

            Debug.DrawRay(g.transform.position, dir * hit.distance, Color.red, 1f);

        }
        else
        {
            Debug.DrawRay(g.transform.position, dir * rayDistance, Color.white, 1f);
        }
    }
}

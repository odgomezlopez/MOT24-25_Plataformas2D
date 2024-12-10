using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(ActorController)), RequireComponent(typeof(Rigidbody2D))]
public class ActorCollision2D : MonoBehaviour
{
    //Referencia al controlador
    ActorController controller;
    Collider2D col2D;
    SpriteRenderer spriteRenderer;

    //Parametros de control
    [SerializeField, Range(1,120)]private int frameRate = 1;
    [SerializeField] private string floorLayer = "Ground";

    private void Start()
    {
        controller = GetComponent<ActorController>();
        col2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % frameRate == 0)
        {
            controller.stateInfo.isFlipped = spriteRenderer.flipX;//If you flip the sprite or player with other method, update this line

            controller.stateInfo.isGrounded.CurrentValue = IsGrounded();
            controller.stateInfo.hasWallFound.CurrentValue = HasWallFound();
            controller.stateInfo.hasFallFound.CurrentValue = HasFallFound();

        }
    }

    private bool IsGrounded()
    {
        // Get collider bounds and calculate raycast origins
        Vector2 colliderBottom = new Vector2(transform.position.x, transform.position.y - col2D.bounds.extents.y);
        Vector2 originLeft = new Vector2(colliderBottom.x - col2D.bounds.extents.x, colliderBottom.y);
        Vector2 originRight = new Vector2(colliderBottom.x + col2D.bounds.extents.x, colliderBottom.y);

        // Scale ray distance proportionally to the collider height
        float scaledRayDistance = col2D.bounds.extents.y * 1.1f; // Slightly longer than the collider to account for uneven surfaces

        // Perform raycasts
        bool hitCenter = Physics2D.Raycast(colliderBottom, Vector2.down, scaledRayDistance, LayerMask.GetMask(floorLayer));
        bool hitLeft = Physics2D.Raycast(originLeft, Vector2.down, scaledRayDistance, LayerMask.GetMask(floorLayer));
        bool hitRight = Physics2D.Raycast(originRight, Vector2.down, scaledRayDistance, LayerMask.GetMask(floorLayer));

        // Debug rays for visualization
        /*Debug.DrawRay(colliderBottom, Vector2.down * scaledRayDistance, Color.red, 0.2f);
        Debug.DrawRay(originLeft, Vector2.down * scaledRayDistance, Color.green, 0.2f);
        Debug.DrawRay(originRight, Vector2.down * scaledRayDistance, Color.blue, 0.2f);*/

        // Return true if any ray hits the ground
        return hitCenter || hitLeft || hitRight;
    }

    private bool HasWallFound()
    {
        //Variables
        float rayDistance = 1.1f; //col2D.bounds.extents.y * 1.1f;

        Vector2 direction = transform.right;
        if (controller.stateInfo.isSpriteFlippedByDefault ^ controller.stateInfo.isFlipped) direction *= -1;

        //controller.stateInfo.isFlipped


        //Calculos
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, LayerMask.GetMask(floorLayer));
        
        //Debug
        Debug.DrawRay(transform.position, direction * rayDistance, hit ? Color.red : Color.blue, 0.2f);

        if (hit) return true;
        else return false;
        //return hit;
    }

    private bool HasFallFound()
    {
        //Variables
        float rayDistance = 2f;

        Vector2 org = transform.position;
        if (controller.stateInfo.isSpriteFlippedByDefault ^ controller.stateInfo.isFlipped) org.x -= col2D.bounds.extents.x;
        else org.x += col2D.bounds.extents.x;

        //Calculos
        RaycastHit2D hit = Physics2D.Raycast(org, Vector2.down, rayDistance, LayerMask.GetMask(floorLayer));

        //Debug
        Debug.DrawRay(org, Vector2.down, !hit ? Color.red : Color.blue, 0.2f);


        if (!hit) return true;
        else return false;
    }
}

using UnityEngine;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class RayCastChecker2D : MonoBehaviour
{
    [Header("Control Parameters")]
    [SerializeField, Range(1, 120)] private int frameRate = 1;
    [SerializeField] private LayerMask floorLayer; //GROUND

    [Header("Actor Collision State Info")]
    public SmartVariable<bool> isGrounded;
    public SmartVariable<bool> hasWallFound;
    public SmartVariable<bool> hasFallFound;

    // Cached references
    [SerializeField] private Collider2D col2D;
    private Rigidbody2D rb;

    private void Start()
    {
        if(!col2D) col2D = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Update at a reduced frequency for performance
        if (Time.frameCount % frameRate == 0)
        {
            isGrounded.CurrentValue = IsGrounded();
            hasWallFound.CurrentValue = HasWallFound();
            hasFallFound.CurrentValue = HasFallFound();
        }
    }

    /// <summary>
    /// Checks if the character is on the ground by casting rays from underneath the collider.
    /// </summary>
    private bool IsGrounded()
    {
        Vector2 colliderBottom = new Vector2(transform.position.x, transform.position.y - col2D.bounds.extents.y);
        Vector2 originLeft = colliderBottom - new Vector2(col2D.bounds.extents.x, 0f);
        Vector2 originRight = colliderBottom + new Vector2(col2D.bounds.extents.x, 0f);

        // Slightly longer ray to account for uneven surfaces
        float rayDistance = col2D.bounds.extents.y * 1.1f;

        bool hitCenter = Physics2D.Raycast(colliderBottom, Vector2.down, rayDistance, floorLayer);
        bool hitLeft = Physics2D.Raycast(originLeft, Vector2.down, rayDistance, floorLayer);
        bool hitRight = Physics2D.Raycast(originRight, Vector2.down, rayDistance, floorLayer);

        return hitCenter || hitLeft || hitRight;
    }

    /// <summary>
    /// Checks if there is a wall in the direction of horizontal movement.
    /// </summary>
    private bool HasWallFound()
    {
        float rayDistance = 1.1f;
        float horizontalVx = rb.linearVelocity.x;

        // Determine direction based on velocity sign
        // (If velocity is near zero, default to right to avoid flicker�customize as needed)
        Vector2 direction = horizontalVx < 0 ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayDistance, floorLayer);

        if (hit.collider) //On Wall
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red, 1f);
            return true;
        }
        else  //No Wall
        {
            Debug.DrawRay(transform.position, direction * rayDistance, Color.white);
            return false;
        }
    }

    /// <summary>
    /// Checks if there is a fall ahead by casting a downward ray from the leading edge.
    /// </summary>
    private bool HasFallFound()
    {
        float rayDistance = Mathf.Min(col2D.bounds.extents.y * 1.5f,1.5f); //1.5f;
        float horizontalVx = rb.linearVelocity.x;
        float safetyModifier = 1.5f;

        

        // Shift the ray origin left or right based on velocity sign
        Vector2 origin = transform.position;
        if (horizontalVx < 0)
            origin.x -= (col2D.bounds.extents.x * safetyModifier);
        else
            origin.x += (col2D.bounds.extents.x * safetyModifier);

        origin.y -= col2D.bounds.extents.y;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayDistance, floorLayer);
        if (hit.collider) //On Ground
        {
            Debug.DrawRay(origin, Vector2.down * hit.distance, Color.white,1f);
            return false;
        } 
        else  //Fall detected
        {
            Debug.DrawRay(origin, Vector2.down * rayDistance, Color.red,1f);
            return true;
        }

        //return hit.collider == null;  // True if no ground is found => there's a fall
    }


}

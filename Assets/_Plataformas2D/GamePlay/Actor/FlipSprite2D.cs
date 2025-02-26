using UnityEngine;

public class FlipSprite2D : MonoBehaviour
{
    //Enum para indicar la dirección
    public enum FacingDirection { Right, Left }

    //Dependencas
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private FacingDirection defaultDirection = FacingDirection.Right;
    [SerializeField] private FacingDirection currentDirection;

    /// <summary>
    /// True if the current facing direction is Right.
    /// </summary>
    public bool IsFacingRight => currentDirection == FacingDirection.Right;

    /// <summary>
    /// True if the current facing direction is Left.
    /// </summary>
    public bool IsFacingLeft => currentDirection == FacingDirection.Left;


    public Vector2 Direction => (IsFacingRight) ? Vector2.right : Vector2.left;

    public Transform FlippedTransform => spriteRenderer.transform;

    public FacingDirection DefaultDirection { get => defaultDirection; set => defaultDirection = value; }

    private void Awake()
    {
        if (!spriteRenderer)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Initialize current direction to the default.
        currentDirection = DefaultDirection;
        ApplyFlip();
    }

    /// <summary>
    /// Flips the character to face the given direction.
    /// </summary>
    public void Flip(FacingDirection newDirection)
    {
        if (currentDirection == newDirection)
            return;

        currentDirection = newDirection;
        ApplyFlip();
    }

    /// <summary>
    /// Flips the sprite based on a speed value, such as rb.velocity.x.
    /// If speed > 0 => Right; if speed < 0 => Left.
    /// </summary>
    public void Flip(float speed)
    {
        if (speed > 0f)
        {
            Flip(FacingDirection.Right);
        }
        else if (speed < 0f)
        {
            Flip(FacingDirection.Left);
        }
        // If speed == 0, do nothing—or you could choose a default orientation.
    }

    /// <summary>
    /// Toggles between Left and Right.
    /// </summary>
    public void ToggleFacing()
    {
        currentDirection = (currentDirection == FacingDirection.Right)
            ? FacingDirection.Left
            : FacingDirection.Right;

        ApplyFlip();
    }

    /// <summary>
    /// Applies the correct flipX state based on currentDirection vs defaultDirection.
    /// </summary>
    private void ApplyFlip()
    {
        // Get current scale
        Quaternion rotation = spriteRenderer.transform.rotation;

        // Make x positive if facing right; negative if facing left
        rotation.y = (currentDirection == FacingDirection.Right) ? 0 : 180;

        // Assign the result
        spriteRenderer.transform.rotation = rotation;

        //Otras opciones descartadas: flipear la escala o rotación del objeto principal, provoca fallos en la fisicas a no ser que se ponga el tipo de colisión a continua. lo cual afecta significativamente al rendimiento.
        //Por tanto, vamos a flipear el spriteRender, junto con sus elementos hijos con rotation o scale. Si fuera con flipX, no se rotan los hijos y complica la lógica del resto de elementos.
    }
}

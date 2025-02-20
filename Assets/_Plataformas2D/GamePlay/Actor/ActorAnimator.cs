using UnityEngine;

[RequireComponent(typeof(RayCastChecker2D)),RequireComponent(typeof(FlipSprite2D))]
public class ActorAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private FlipSprite2D flipSprite2D;
    private RayCastChecker2D rayCastInfo;

    // You can adjust this threshold to control when flipping occurs
    [SerializeField] private float flipThreshold = 0.1f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        flipSprite2D = GetComponent<FlipSprite2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rayCastInfo = GetComponent<RayCastChecker2D>();
    }

    private void FixedUpdate()
    {
        float xVel = rb.linearVelocityX;
        float yVel = rb.linearVelocityY;

        animator.SetFloat("VelocityX", Mathf.Abs(xVel));
        animator.SetFloat("VelocityY", yVel);
        animator.SetBool("IsGrounded", rayCastInfo.isGrounded.CurrentValue);

        // Flip if horizontal velocity exceeds threshold
        if (Mathf.Abs(xVel) > flipThreshold)
        {
            flipSprite2D.Flip(xVel);
        }
    }
}

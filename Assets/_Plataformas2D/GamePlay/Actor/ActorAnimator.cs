using UnityEngine;

[RequireComponent(typeof(RayCastChecker2D))]
public class ActorAnimator : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private RayCastChecker2D rayCastInfo;

    // You can adjust this threshold to control when flipping occurs
    [SerializeField] private float flipThreshold = 0.1f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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
            Flip(xVel > 0);
        }
    }

    private void Flip(bool isFacingRight)
    {
        // Flip by adjusting localScale.x
        var newScale = sr.transform.localScale;
        if (isFacingRight) newScale.x = Mathf.Abs(newScale.x);
        else newScale.x = -Mathf.Abs(newScale.x);
        sr.transform.localScale = newScale;

        //Version antigua
        //sr.flipX = !isFacingRight;
    }
}

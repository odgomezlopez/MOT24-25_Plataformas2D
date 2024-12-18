using UnityEngine;

[RequireComponent(typeof(RayCastChecker2D))]
public class ActorAnimator : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    RayCastChecker2D rayCastInfo;

    float lastX=0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer= GetComponentInChildren<SpriteRenderer>();
        rayCastInfo = GetComponent<RayCastChecker2D>();

        lastX = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (Time.frameCount % 5 == 0)
        animator.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("VelocityY", rb.linearVelocity.y);
        animator.SetBool("IsGrounded", rayCastInfo.isGrounded.CurrentValue);

        if (Mathf.Abs(rb.linearVelocity.x) > 0.1) Flip(rb.linearVelocity.x > 0);
        //if(Mathf.Abs(lastX - transform.position.x)>0.2) Flip(lastX < transform.position.x);
        //lastX = transform.position.x;
    }

    private void Flip(bool isFacingRight)
    {
        spriteRenderer.flipX = !isFacingRight;
    }
}

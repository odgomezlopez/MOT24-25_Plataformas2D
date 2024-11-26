using UnityEngine;

public class ActorAnimator : MonoBehaviour
{
    ActorController controller;
    Animator animator;
    Rigidbody2D rb;

    void Start()
    {
        controller = GetComponent<ActorController>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Time.frameCount % 5 == 0)
        animator.SetFloat("VelocityX",Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("VelocityY", rb.linearVelocity.y);
        animator.SetBool("IsGrounded", controller.stateInfo.isGrounded);
    }
}

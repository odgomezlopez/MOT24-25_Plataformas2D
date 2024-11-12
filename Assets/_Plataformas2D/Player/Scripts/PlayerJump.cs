using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerJump : MonoBehaviour
{
    //Parametros
    [SerializeField] float jumpForze = 5f;
    [SerializeField] bool jumpPressed = false;

    //Ref. Componentes
    Rigidbody2D fisicas;
    CapsuleCollider2D capsuleCollider;

    //Eventos
    [SerializeField] UnityEvent OnJump;

    void Start()
    {
        fisicas = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(IsGrounded()) jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        if (jumpPressed)
        {
            OnJump.Invoke();
            fisicas.AddForce(Vector2.up * jumpForze, ForceMode2D.Impulse);
            jumpPressed = false;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1.1f, LayerMask.GetMask("Ground"));

        if (hit)
        {
            Debug.DrawRay(transform.position, -Vector2.up * 1.1f, Color.red, 0.2f);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, -Vector2.up * 1.1f, Color.white, 0.2f);
            return false;
        }
    }
}

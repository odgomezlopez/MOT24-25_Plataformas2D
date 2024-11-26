﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerController)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerLateralMovement2D : MonoBehaviour
{
    //Componentes referencia
    PlayerController controller;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    //Accesos rápidos
    PlayerStats Stats => (PlayerStats) controller.Stats;

    //Variables internas
    private float inputX;
    private bool jumpPressed = false;


    [Header("Acciones")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference runAction;
    [SerializeField] InputActionReference jumpAction;

    //Eventos
    [Header("Eventos")]
    [SerializeField] UnityEvent OnJump;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Referencias
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        //controller.Stats.HP = 3;
        Stats.HP = 3;
    }

    // Update is called once per frame
    void Update()
    {
        /*Comprobaciones de movimiento*/
        inputX = moveAction.action.ReadValue<Vector2>().x; //Input.GetAxis("Horizontal");
        //transform.position += Vector3.right * inputX * speed * Time.deltaTime;

        if (Time.frameCount % 5 == 0) CheckFlip(inputX); //Permite que la comprobaci�n de la direcci�n se ejecute cada 5 frames

        /*Comprobaciones de salto*/
        if (jumpAction.action.triggered) //Input.GetKeyDown(KeyCode.Space)
        {
            if (IsGrounded()) jumpPressed = true;
        }
    }

    private void FixedUpdate()
    {
        /*Movimiento*/
        MoveFixedUpdate();

        /*Salto*/
        JumpFixedUpdate();
    }

    private void MoveFixedUpdate()
    {
        float runModifier = runAction.action.IsPressed() ? Stats.runSpeedModifier : 1f; //Obtego el modificador de correr

        rb.linearVelocity = new Vector2(inputX * Stats.speed * runModifier , rb.linearVelocity.y);

        //rb.AddForce();//TODO Actualmente pierde la inercia previa del salto, arreglar.

        float airMomentumModifier = 1;
        if (!IsGrounded()) airMomentumModifier = Stats.airMomentumModifier;
    }

    private void JumpFixedUpdate()
    {
        if (jumpPressed)
        {
            OnJump.Invoke();
            rb.AddForce(Vector2.up * Stats.jumpForce, ForceMode2D.Impulse);
            jumpPressed = false;
        }
    }


    #region Funciones de utilidad de movimiento 2D
    private void CheckFlip(float dir)
    {
        if (dir == 0) return; //Si no nos movemos, no hacemos ninguna comprobaci�n m�s

        if (dir > 0) spriteRenderer.flipX = false;
        if (dir < 0) spriteRenderer.flipX = true;
    }

    /* Version menos eficiente ya que requiere varios calculos con vectoes y de operaciones matematicas como el valor absoluto
    private void CheckFlip(float dir)
    {
        if (dir == 0) return; //Si no nos movemos, no hacemos ninguna comprobaci�n m�s

        Vector3 newScale = transform.localScale;

        if (dir > 0) newScale.x = Math.Abs(newScale.x);
        if (dir < 0) newScale.x = -Math.Abs(newScale.x);

        spriteRenderer.transform.localScale = newScale;
    }
    */
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
    #endregion
}

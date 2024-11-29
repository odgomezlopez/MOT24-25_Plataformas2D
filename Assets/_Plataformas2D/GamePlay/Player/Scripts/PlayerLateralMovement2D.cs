﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(PlayerController)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerLateralMovement2D : MonoBehaviour
{
    //Componentes referencia
    PlayerController controller;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    //Accesos rápidos
    PlayerStats Stats => (PlayerStats) controller.Stats;

    [Header("Acciones")]
    //Movimiento
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference runAction;

    //[SerializeField] InputActionReference dashAction;

    //Salto
    [SerializeField] InputActionReference jumpAction;

    //Variables internas
    //Movimiento
    private float inputX;
    //Salto
    private bool jumpPressed = false;


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

    
    private void OnEnable()
    {
        if (moveAction?.action != null)
        {
            moveAction.action.performed += OnMoveInput;
            moveAction.action.canceled += OnMoveInput;
        }
        if (jumpAction?.action != null)
        {
            jumpAction.action.performed += OnJumpInput;
        }
        /*if (dashAction?.action != null)
        {
            dashAction.action.performed += OnDashInput;
        }*/
    }

    private void OnDisable()
    {
        if (moveAction?.action != null)
        {
            moveAction.action.performed -= OnMoveInput;
            moveAction.action.canceled -= OnMoveInput;
        }
        if (jumpAction?.action != null)
        {
            jumpAction.action.performed -= OnJumpInput;
        }
        /*if (dashAction?.action != null)
        {
            dashAction.action.performed -= OnDashInput;
        }*/
    }
    
    
    // Update is called once per frame
    void Update()
    {
        //OnMoveInput();
        //OnJumpInput();
    }
    #region Update Methods
    public void OnMoveInput(InputAction.CallbackContext context = default)
    {
        /*Comprobaciones de movimiento*/

        inputX = moveAction.action.ReadValue<Vector2>().x; //Input.GetAxis("Horizontal");

        //transform.position += Vector3.right * inputX * speed * Time.deltaTime;
        Debug.Log(inputX);
        //if (Time.frameCount % 5 == 0) 
        CheckFlip(inputX); //Permite que la comprobaci�n de la direcci�n se ejecute cada 5 frames
    }

    public void OnJumpInput(InputAction.CallbackContext context = default)
    {
        /*Comprobaciones de salto*/
        if (jumpAction.action.triggered) //Input.GetKeyDown(KeyCode.Space)
        {
            if (controller.stateInfo.isGrounded) jumpPressed = true;
        }
    }

    /*public void OnDashInput(InputAction.CallbackContext context = default)
    {
        StartCoroutine(Dash());
    }*/

    #endregion

    private void FixedUpdate()
    {
        /*Movimiento*/
        MoveFixedUpdate();

        /*Salto*/
        JumpFixedUpdate();

        /*Gravedad*/
        AdjustGravity();
    }
    #region FixedUpdateMethods

    private void MoveFixedUpdate()
    {
        // Obtengo la velocidad actual
        float currentSpeedX = rb.linearVelocityX;
        if(currentSpeedX*inputX < 0) // if (Mathf.Sign(currentSpeedX) != Mathf.Sign(inputX) && inputX != 0)
            currentSpeedX = 0; //Si cambio de dirección, pongo la velocidad actual a 0

        // Obtengo la velocidad objetivo, dependiendo de si estoy o no corriendo
        float targetSpeedX = inputX * Stats.GetComputedSpeed(runAction.action.IsPressed());

        //Obtengo la acceleración o deceleración. Depende de si estoy o no en el aire
        float accelerationRate = (inputX != 0)
            ? Stats.GetComputedAccelerationSeconds(controller.stateInfo.isGrounded)
            : Stats.GetComputedDeccelerationSeconds();

        // Calculo la nueva velocidad utilizando MoveTowards.
        rb.linearVelocityX = Mathf.MoveTowards(currentSpeedX, targetSpeedX, accelerationRate * Time.fixedDeltaTime);
        //Debug.Log("accelerationRate: " + accelerationRate);
        //Debug.Log("VelocidadX: " + rb.linearVelocityX);
    }

    private void JumpFixedUpdate()
    {
        //Gestión Salto
        if (jumpPressed)
        {
            OnJump.Invoke();
            //rb.AddForce(Vector2.up * Stats.jumpForce, ForceMode2D.Impulse);
            rb.linearVelocityY = Stats.jumpForce;
            jumpPressed = false;
        }
    }

    private void AdjustGravity()
    {
        rb.gravityScale = !controller.stateInfo.isGrounded && rb.linearVelocityY < 0
            ? Stats.gravityScaleFalling
            : Stats.gravityScaleDefault;
    }

    /*private IEnumerator Dash()
    {
        // Dash logic
        //canDash = false; // Prevent multiple dashes
        float originalGravity = rb.gravityScale; // Store original gravity
        float dashDirection = spriteRenderer.flipX ? -1f : 1f; // -1 for left, 1 for right

        rb.gravityScale = 0; // Temporarily disable gravity
        rb.linearVelocity = new Vector2(dashDirection * Stats.dashForce, 0); // Apply dash force
        yield return new WaitForSeconds(0.2f); // Dash duration

        rb.gravityScale = originalGravity; // Restore gravity
        yield return new WaitForSeconds(Stats.dashCooldown - 0.2f); // Cooldown period
        //canDash = true; // Allow dashing again
    }*/
    #endregion

    #region Funciones de utilidad de movimiento 2D
    private void CheckFlip(float dir)
    {
        if (dir != 0)
            spriteRenderer.flipX = dir < 0;
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

    #endregion
}

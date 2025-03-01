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
    FlipSprite2D flipSprite2D;

    Rigidbody2D rb;
    RayCastChecker2D rayCastInfo;
    ActorAnimator actorAnimator;

    //Accesos rápidos
    PlayerStats Stats => (PlayerStats) controller.Stats;

    [Header("Acciones")]
    //Movimiento
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference runAction;

    [SerializeField] InputActionReference dashAction;

    //Salto
    [SerializeField] InputActionReference jumpAction;

    //Variables internas
    //Movimiento
    private float inputX;
    //Salto
    private bool jumpPressed = false;
    [SerializeField] private int jumpNumPerfomed = 0;

    //Eventos
    [Header("Eventos")]
    [SerializeField] UnityEvent OnJump;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //Referencias
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rayCastInfo = GetComponent<RayCastChecker2D>();
        flipSprite2D = GetComponent<FlipSprite2D>();
        actorAnimator = GetComponent<ActorAnimator>();
        //controller.Stats.HP = 3;
        Stats.HP = 3;
        jumpNumPerfomed = 0;
    }


    private void OnEnable()
    {
        //Subscribirnos a las acciones del jugador
        if (moveAction?.action != null)
        {
            moveAction.action.performed += OnMoveInput;
            moveAction.action.canceled += OnMoveInput;
        }
        if (jumpAction?.action != null)
        {
            jumpAction.action.performed += OnJumpInput;
        }
        if (dashAction?.action != null)
        {
            dashAction.action.performed += OnDashInput;
        }

        //Subscribirnos al evento de tocar el suelo
        rayCastInfo.isGrounded.OnValueUpdate.AddListener(ResetJumps);
    }

    private void OnDisable()
    {
        //Desubscribirnos a las acciones del jugador
        if (moveAction?.action != null)
        {
            moveAction.action.performed -= OnMoveInput;
            moveAction.action.canceled -= OnMoveInput;
        }
        if (jumpAction?.action != null)
        {
            jumpAction.action.performed -= OnJumpInput;
        }
        if (dashAction?.action != null)
        {
            dashAction.action.performed -= OnDashInput;
        }

        //Desubscribirnos al evento de tocar el suelo
        rayCastInfo.isGrounded.OnValueUpdate.RemoveListener(ResetJumps);
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
        //Debug.Log(inputX);
        //if (Time.frameCount % 5 == 0) 
        //CheckFlip(inputX); //Permite que la comprobaci�n de la direcci�n se ejecute cada 5 frames
    }

    public void OnJumpInput(InputAction.CallbackContext context = default)
    {
        /*Comprobaciones de salto*/
        if (jumpAction.action.triggered) //Input.GetKeyDown(KeyCode.Space)
        {
            if (rayCastInfo.isGrounded.CurrentValue || jumpNumPerfomed < Stats.jumpNumMax)
            {
                jumpNumPerfomed++;
                jumpPressed = true;
            }
        }
    }

    public void ResetJumps(bool onGround)
    {
        if (onGround) jumpNumPerfomed = 0;
    }

    public void OnDashInput(InputAction.CallbackContext context = default)
    {
        StartCoroutine(Dash());
    }

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
        //Debug.Log("VelocidadX: " + rb.linearVelocityX);

        // Obtengo la velocidad actual
        float currentSpeedX = rb.linearVelocityX;
        if(currentSpeedX*inputX < 0) // if (Mathf.Sign(currentSpeedX) != Mathf.Sign(inputX) && inputX != 0)
            currentSpeedX = 0; //Si cambio de dirección, pongo la velocidad actual a 0

        // Obtengo la velocidad objetivo, dependiendo de si estoy o no corriendo
        float targetSpeedX = inputX * Stats.GetComputedSpeed(runAction.action.IsPressed());

        //(Opcional) Comprobación para ver si ya estoy a la velocidad objetivo o muy cerca de ella
        if(Mathf.Abs(currentSpeedX - targetSpeedX) < 0.1f)
        {
            rb.linearVelocityX = targetSpeedX;
            return;
        }

        //Sino, obtengo la acceleración o deceleración. Depende de si estoy o no en el aire
        float accelerationRate = (inputX != 0)
            ? Stats.GetComputedAccelerationSeconds(rayCastInfo.isGrounded.CurrentValue)
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
        if (!rayCastInfo.isGrounded.CurrentValue && rb.linearVelocityY < 0)
            rb.gravityScale = Stats.gravityScaleFalling;
        else 
            rb.gravityScale = Stats.gravityScaleDefault;
    }

    private IEnumerator Dash()
    {
        // Disable the dash input so we can't dash again immediately
        dashAction.action.Disable();

        // Start dash animation
        actorAnimator.SetTrigger("DashStart");
        yield return new WaitForSeconds(0.1f);

        // Disable collisions between player and enemy layers
        int layerA = LayerMask.NameToLayer("Player");
        int layerB = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(layerA, layerB, true);

        // Store original gravity and figure out which way to dash
        float originalGravity = rb.gravityScale;
        float dashDirection = flipSprite2D.IsFacingRight ? 1 : -1;

        // Temporarily turn off gravity
        rb.gravityScale = 0f;

        // Dash for a duration using a time-based loop
        float elapsed = 0f;
        while (elapsed < Stats.dashDuration)
        {
            rb.linearVelocity = new Vector2(dashDirection * Stats.dashVelocity, 0f);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Restore normal movement
        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;

        // Re-enable collisions
        Physics2D.IgnoreLayerCollision(layerA, layerB, false);

        // Signal the end of the dash
        actorAnimator.SetTrigger("DashFinish");

        // (Optional) If you want a separate cooldown before the next dash:
        // yield return new WaitForSeconds(Stats.dashCooldown);

        // Re-enable the dash input
        dashAction.action.Enable();
    }

    #endregion

    #region Funciones de utilidad de movimiento 2D
    /*private void CheckFlip(float dir)
    {
        if (dir != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(dir) * Mathf.Abs(transform.localScale.x),
                                   transform.localScale.y,
                                   transform.localScale.z);
            //spriteRenderer.flipX = dir < 0;
        }
    }*/

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

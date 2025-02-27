using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RayCastChecker2D)),RequireComponent(typeof(FlipSprite2D))]
public class ActorAnimator : MonoBehaviour
{
    private Animator animator;
    private AnimatorParameterCache parameterCache;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private FlipSprite2D flipSprite2D;
    private RayCastChecker2D rayCastInfo;

    // You can adjust this threshold to control when flipping occurs
    [SerializeField] private InputActionReference inputAction;
    [SerializeField] Vector2 input;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        flipSprite2D = GetComponent<FlipSprite2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rayCastInfo = GetComponent<RayCastChecker2D>();

        parameterCache = new(animator);
    }

    private void OnEnable()
    {
        //Subscribirnos a las acciones del jugador
        if (inputAction?.action != null)
        {
            inputAction.action.performed += OnMoveInput;
            inputAction.action.canceled += OnMoveInput;
        }
    }

    private void OnDisable()
    {
        if (inputAction?.action != null)
        {
            inputAction.action.performed -= OnMoveInput;
            inputAction.action.canceled -= OnMoveInput;
        }
    }

    public void OnMoveInput(InputAction.CallbackContext context = default)
    {
        input = inputAction.action.ReadValue<Vector2>(); //Input.GetAxis("Horizontal");
    }

    public void SetTrigger(string trigger) {
        if (parameterCache.HasParameter(animator, trigger)) animator.SetTrigger(trigger);
    }
    public void SetBool(string parameter, bool state)
    {
        if (parameterCache.HasParameter(animator, parameter)) animator.SetBool(parameter, state);
    }

    private void FixedUpdate()
    {
        float xVel = rb.linearVelocityX;
        float yVel = rb.linearVelocityY;

        if(parameterCache.HasParameter(animator,"VelocityX")) animator.SetFloat("VelocityX", Mathf.Abs(xVel));
        if (parameterCache.HasParameter(animator, "VelocityY")) animator.SetFloat("VelocityY", yVel);
        if (parameterCache.HasParameter(animator, "IsGrounded")) animator.SetBool("IsGrounded", rayCastInfo.isGrounded.CurrentValue);
        if (parameterCache.HasParameter(animator, "hasWallFound")) animator.SetBool("hasWallFound", rayCastInfo.hasWallFound.CurrentValue);
        if (parameterCache.HasParameter(animator, "hasFallFound")) animator.SetBool("hasFallFound", rayCastInfo.hasFallFound.CurrentValue);

        if (parameterCache.HasParameter(animator, "InputX")) animator.SetFloat("InputX", input.x);
        if (parameterCache.HasParameter(animator, "InputY")) animator.SetFloat("InputY", input.y);

        // Flip if horizontal velocity exceeds threshold
        if (Mathf.Abs(xVel) > 0.1)
        {
            flipSprite2D.Flip(xVel);
        }
    }
}


public class AnimatorParameterCache
{
    private Dictionary<int, bool> parameterCache = new Dictionary<int, bool>();

    public AnimatorParameterCache(Animator animator)
    {
        if (animator == null) return;

        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            parameterCache[param.nameHash] = true;
        }
    }

    public bool HasParameter(Animator animator, string paramName)
    {
        return parameterCache.ContainsKey(Animator.StringToHash(paramName));
    }
}
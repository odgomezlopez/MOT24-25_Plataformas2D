using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;


[RequireComponent(typeof(PlayerController))]
public class PlayerActionController : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference action1;
    [SerializeField] InputActionReference action2;

    //Variables
    private Vector2 input;

    //Dependencias
    PlayerController playerController;
    PlayerStats stats => (PlayerStats) playerController.Stats;

    SpriteRenderer sprite;
    FlipSprite2D flipSprite2D;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
        flipSprite2D = GetComponent<FlipSprite2D>();
    }

    private void OnEnable()
    {
        //Subscribirnos a las acciones del jugador
        if (moveAction?.action != null)
        {
            moveAction.action.performed += OnMoveInput;
            moveAction.action.canceled += OnMoveInput;
        }

        action1.action.Enable();
        action2.action.Enable();

        action1.action.performed += ExecuteAction1;
        action2.action.performed += ExecuteAction2;
    }

    private void OnDisable()
    {
        if (moveAction?.action != null)
        {
            moveAction.action.performed -= OnMoveInput;
            moveAction.action.canceled -= OnMoveInput;
        }

        action1.action.performed -= ExecuteAction1;
        action2.action.performed -= ExecuteAction2;

    }

    public void OnMoveInput(InputAction.CallbackContext context = default)
    {
        input = moveAction.action.ReadValue<Vector2>(); //Input.GetAxis("Horizontal");
    }


    private void ExecuteAction1(InputAction.CallbackContext context)
    {
        if (stats.action1)
        {
            Debug.Log(input);
            if(stats.action1Up & input.y > 0.5) stats.action1Up.Use(gameObject);
            else if (stats.action1Down & input.y < -0.5) stats.action1Down.Use(gameObject);
            else stats.action1.Use(gameObject);
        }

        StartCoroutine(CoolDown(action1.action, stats.action1.delay));
    }

    private void ExecuteAction2(InputAction.CallbackContext context)
    {
        if (stats.action2)
        {
            stats.action2.Use(gameObject);
        }

        StartCoroutine(CoolDown(action2.action, stats.action2.delay));
    }

    private IEnumerator CoolDown(InputAction action, float coolDownSeconds)
    {
        action.Disable();
        yield return new WaitForSeconds(coolDownSeconds);
        action.Enable();
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(PlayerController))]
public class PlayerActionController : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] InputActionReference action1;
    [SerializeField] InputActionReference action2;

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
        action1.action.Enable();
        action2.action.Enable();

        action1.action.performed += ExecuteAction1;
        action2.action.performed += ExecuteAction2;
    }

    private void OnDisable()
    {
        action1.action.performed -= ExecuteAction1;
        action2.action.performed -= ExecuteAction2;

    }

    private void ExecuteAction1(InputAction.CallbackContext context)
    {
        if (stats.action1)
        {
            stats.action1.Use(gameObject);
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

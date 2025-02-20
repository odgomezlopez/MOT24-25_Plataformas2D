using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


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

    private void ExecuteAction1(InputAction.CallbackContext context=default)
    {
        Debug.Log("Execute Action 1");
        if (stats.action1)
        {
            //Instanciamos el ataque dentro del padre
            GameObject g = Instantiate(stats.action1, flipSprite2D.FlippedTransform);
            //g.layer = gameObject.layer;
            LayerHelper.SetLayerRecursively(g, gameObject.layer);

            var hitBox2D = g.GetComponent<HitBox2D>();
            if (hitBox2D)
            {
                hitBox2D.Origin = playerController;
                hitBox2D.Damage = 2;
                hitBox2D.SetTimeLimitDestroy(10f);
            }
        }

        StartCoroutine(CoolDown(action1.action, 0.5f));
    }

    private void ExecuteAction2(InputAction.CallbackContext context)
    {
        if (stats.action2)
        {
            //Instanciamos el ataque dentro del padre
            GameObject g = Instantiate(stats.action2, transform.position, transform.rotation);
            //g.layer = gameObject.layer;
            LayerHelper.SetLayerRecursively(g, gameObject.layer);

            var hitBox2D = g.GetComponent<HitBox2D>();
            if (hitBox2D)
            {
                hitBox2D.Origin = playerController;
                hitBox2D.Damage = 1;
                hitBox2D.SetTimeLimitDestroy(10f);
            }
            var moveFowards2D = g.GetComponent<MoveFowards2D>();
            if (moveFowards2D)
            {
                moveFowards2D.Speed = 20;
                moveFowards2D.MoveRight = flipSprite2D.IsFacingRight;//sprite.transform.localScale.x > 0; //NOTA. Debe tener en cuenta como rotamos el sprite del jugador
            }

        }

        StartCoroutine(CoolDown(action2.action, 1f));
    }

    private IEnumerator CoolDown(InputAction action, float coolDownSeconds)
    {
        action.Disable();
        yield return new WaitForSeconds(coolDownSeconds);
        action.Enable();
    }
}

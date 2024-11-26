using UnityEngine;

[RequireComponent(typeof(ActorController)), RequireComponent(typeof(Rigidbody2D))]
public class ActorCollision : MonoBehaviour
{
    //Referencia al controlador
    ActorController controller;

    //Parametros de control
    [SerializeField, Range(1,10)]private int frameRate = 1;
    [SerializeField] private string floorLayer = "Ground";
    [SerializeField] private float rayDistance = 1.1f;

    private void Start()
    {
        controller = GetComponent<ActorController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % frameRate == 0)
        {
            controller.stateInfo.isGrounded = IsGrounded();
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, rayDistance, LayerMask.GetMask(floorLayer));

        if (hit)
        {
            Debug.DrawRay(transform.position, -Vector2.up * rayDistance, Color.red, 0.2f);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, -Vector2.up * rayDistance, Color.white, 0.2f);
            return false;
        }
    }
}

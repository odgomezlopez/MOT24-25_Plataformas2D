using UnityEngine;

public class MoveFowards2D : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed = 5;
    [SerializeField] bool moveRight = true;


    public float Speed { get => speed; set => speed = value; }
    public bool MoveRight { get => moveRight; set => moveRight = value; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = (MoveRight) ? Speed : -Speed;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerMove : MonoBehaviour
{
    //Variables
    private float inputX;
    [SerializeField] private float speed=5f;

    //Referencias a componentes
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX=Input.GetAxis("Horizontal");
        //transform.position += Vector3.right * inputX * speed * Time.deltaTime;

        if(Time.frameCount % 5 == 0) CheckFlip(inputX); //Permite que la comprobación de la dirección se ejecute cada 5 frames
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX*speed,rb.velocity.y);
    }

    private void CheckFlip(float dir)
    {
        if (dir == 0) return; //Si no nos movemos, no hacemos ninguna comprobación más

        if(dir > 0) spriteRenderer.flipX = false;
        if(dir < 0) spriteRenderer.flipX = true;
    }

    /* Versión menos eficiente ya que requiere varios calculos con vectoes y de operaciones matematicas como el valor absoluto
    private void CheckFlip(float dir)
    {
        if (dir == 0) return; //Si no nos movemos, no hacemos ninguna comprobación más

        Vector3 newScale = transform.localScale;

        if (dir > 0) newScale.x = Math.Abs(newScale.x);
        if (dir < 0) newScale.x = -Math.Abs(newScale.x);

        spriteRenderer.transform.localScale = newScale;
    }
    */
}

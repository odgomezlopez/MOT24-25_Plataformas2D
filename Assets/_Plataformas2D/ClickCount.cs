using Unity.VisualScripting;
using UnityEngine;

public class ClickCount : MonoBehaviour
{
    [Header("Vidas")]
    //Variables
    [SerializeField] private int vidas = 10;
    private int vidasIniciales;

    [Header("Colores")]
    [SerializeField] Color sano = Color.green;
    [SerializeField] Color muerte = Color.red;

    [Header("CoolDown")]
    [SerializeField] private float cooldDownTime = 2f;
    [SerializeField] private float timeSinceLastClick;


    //Referencias
    SpriteRenderer sprite;

    private void Awake()
    {
        vidasIniciales = vidas;
        timeSinceLastClick = 0f;

        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = true;
        sprite.material.color = sano;
    }

    private void Update()
    {
        timeSinceLastClick += Time.deltaTime;
    }


    private void OnMouseDown()
    {
        if (timeSinceLastClick < cooldDownTime) return;
        timeSinceLastClick = 0f;

        vidas--;
        float porcentaje =  1f - ((float) vidas / (float) vidasIniciales);
        var lerpedColor = Color.Lerp(sano, muerte, porcentaje) ;
        sprite.material.color = lerpedColor;

        if (vidas <= 0)
        {
            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        //sprite = GetComponent<SpriteRenderer>();
        //sprite.material.color = sano;

        if (vidas < 1) vidas = 1;  
    }

}

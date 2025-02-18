using UnityEngine;
using UnityEngine.Events;

public class HitBox2D : MonoBehaviour
{
    [Header("Hitbox Config")]
    [SerializeField] float damage = 1;

    [SerializeField] bool disableOnTrigger = true;
    [SerializeField] bool destroyOnTrigger = false;

    //[SerializeField] string wallTag = "Floor";

    [SerializeField] bool triggerOnObstacules = true;


    public float Damage { get => damage; set => damage = value; }

    [SerializeField] UnityEvent OnHit;

    public void SetTimeLimitDestroy(float seconds=5f)
    {
        Destroy(gameObject,seconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Colisiones con actores
        HurtBox2D hurtBox2D = collision.GetComponent<HurtBox2D>();
        if(hurtBox2D != null)
        {
            hurtBox2D.TakeDamage(Damage, gameObject);
            OnHit.Invoke();
        }

        //Colisiones con paredes y suelo
        if (hurtBox2D || triggerOnObstacules)
        {
            if (disableOnTrigger) gameObject.SetActive(false);
            if (destroyOnTrigger) Destroy(gameObject);
            return;
        }
    }


}

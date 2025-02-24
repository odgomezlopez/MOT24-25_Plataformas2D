using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HitBox2D : MonoBehaviour
{
    [Header("Hitbox Config")]
    [SerializeField] float damage = 1;

    [SerializeField] bool disableOnTrigger = true;
    [SerializeField] bool destroyOnTrigger = false;

    [SerializeField] string wallTag = "Floor";
    [SerializeField] bool triggerOnObstacules = true;

    [Header("Origin Data")]
    [SerializeField] ActorController origin;

    public float Damage { get => damage; set => damage = value; }
    public ActorController Origin { get => origin; set => origin = value; }


    [SerializeField] UnityEvent OnHit;

    public void SetTimeLimit(float seconds=5f)
    {
        //Destroy(gameObject,seconds);
        gameObject.Release(seconds);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Colisiones con actores
        HurtBox2D hurtBox2D = collision.GetComponent<HurtBox2D>();
        if(hurtBox2D != null)
        {
            hurtBox2D.TakeDamage(Damage, gameObject,Origin);
            OnHit.Invoke();
        }

        //Colisiones con paredes y suelo
        if (hurtBox2D || (triggerOnObstacules && collision.CompareTag(wallTag)))
        {
            if (ObjectPoolManager.Instance && (disableOnTrigger || destroyOnTrigger))
            {
                gameObject.Release();
                //ObjectPoolManager.Instance.ReturnObject(gameObject);
            }
            else if (disableOnTrigger) gameObject.SetActive(false);
            else if (destroyOnTrigger) Destroy(gameObject);
            return;
        }
    }


}

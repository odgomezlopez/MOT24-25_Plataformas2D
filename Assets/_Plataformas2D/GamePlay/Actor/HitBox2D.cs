using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HitBoxConfig
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 1;

    [Header("Hitbox Behavior")]
    [SerializeField] private bool onlyHitsOnce = true;
    [SerializeField] private bool triggerOnObstacles = true;
    [SerializeField] private string obstacleTag = "Floor";
    [SerializeField] private float extraTime = 0f;

    [Header("Knockback Settings")]
    [SerializeField] private Vector2 knockbackForce = new Vector2(5, 2);

    [Header("Lifetime Settings")]
    [SerializeField] private float hitboxLifetime = 5f;

    //[Header("FX Settings")]
    //[SerializeField] private ParticleSystem hitEffect;
    //[SerializeField] private AudioClip hitSound;

    // Public accessors
    public float Damage { get => damage; set => damage = value; }
    public bool OnlyHitsOnce => onlyHitsOnce;
    public bool TriggerOnObstacles => triggerOnObstacles;
    public string ObstacleTag => obstacleTag;
    public float ExtraTime => extraTime;

    public Vector2 KnockbackForce => knockbackForce;
    public float HitboxLifetime => hitboxLifetime;

    //public ParticleSystem HitEffect => hitEffect;
    //public AudioClip HitSound => hitSound;
}


public class HitBox2D : MonoBehaviour
{
    [Header("HitBox Config")]
    [SerializeField] public HitBoxConfig config;

    [Header("Origin Data")]
    [SerializeField] private ActorController origin;
    public ActorController Origin { get => origin; set => origin = value; }


    //Avoid multipleHit system
    [SerializeField] private float hitCooldown = 0.5f;
    private Dictionary<ActorController, float> lastHitTime = new Dictionary<ActorController, float>();

    //Events
    [SerializeField] private UnityEvent OnHit;

    private void Start()
    {
        //if(config.HitboxLifetime > 0) SetTimeLimit(config.HitboxLifetime);
    }

    /// <summary>
    /// Sets a time limit after which the game object is released.
    /// </summary>
    public void SetTimeLimit(float seconds = 5f)
    {
        gameObject.Release(seconds);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Obtengo referencias
        ActorController actor = collision.transform.GetComponentInParent<ActorController>();
        HurtBox2D hurtBox = collision.transform.GetComponent<HurtBox2D>();

        if (!actor && !hurtBox && !(config.TriggerOnObstacles && collision.CompareTag(config.ObstacleTag))) return;


        //Check if there is cooldown for this object
        if (actor)
        {
            if (lastHitTime.TryGetValue(actor, out float lastTime))
            {
                if (Time.time - lastTime < hitCooldown)
                    return;
            }
            lastHitTime[actor] = Time.time;
        }


        // Check for collisions with hurtboxes or actor controllers.
        if (hurtBox)
            hurtBox.TakeDamage(config.Damage, gameObject, Origin);
        else if (actor)
            actor.TakeDamage(config.Damage, gameObject);
  
        OnHit.Invoke();
        if (config.OnlyHitsOnce) StartCoroutine(WaitToPerformAction(config.ExtraTime));
    }

    private IEnumerator WaitToPerformAction(float extraWaitTime)
    {
        if (extraWaitTime > 0f)
        {
            yield return new WaitForSeconds(extraWaitTime);
        }

        // Execute the configured trigger action.
        gameObject.Release();
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}

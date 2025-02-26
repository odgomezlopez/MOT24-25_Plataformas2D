using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public enum HitBoxTriggerAction
{
    None,
    Release,
    Disable,
    Destroy
}
public class HitBox2D : MonoBehaviour
{
    [Header("Hitbox Config")]
    [SerializeField] private float damage = 1;
    public float Damage { get => damage; set => damage = value; }

    [SerializeField] public HitBoxTriggerAction triggerAction = HitBoxTriggerAction.Release;
    [SerializeField] public string obstacleTag = "Floor";
    [SerializeField] public bool triggerOnObstacles = true;
    [SerializeField] public float extraWaitTime = 0f;

    [Header("Origin Data")]
    [SerializeField] private ActorController origin;
    public ActorController Origin { get => origin; set => origin = value; }

    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent OnHit;


    private void Start()
    {
        // Ensure we have a reference to an animator if one exists in children.
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
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
        bool hit = false;

        // Check for collisions with hurtboxes or actor controllers.
        var hurtBox = collision.GetComponent<HurtBox2D>();
        var actor = collision.GetComponentInParent<ActorController>();

        if (hurtBox != null)
        {
            hurtBox.TakeDamage(Damage, gameObject, Origin);
            hit = true;
        }
        else if (actor != null)
        {
            actor.TakeDamage(Damage, gameObject);
            hit = true;
        }
        // Check for obstacles if enabled.
        else if (triggerOnObstacles && collision.CompareTag(obstacleTag))
        {
            hit = true;
        }

        if (hit)
        {
            OnHit.Invoke();
            StartCoroutine(WaitForAnimationThenPerformAction(extraWaitTime));
        }
    }

    private IEnumerator WaitForAnimationThenPerformAction(float extraWaitTime)
    {
        //if (animator != null)
        //{
        //    // Cache the current animation state and wait until it has finished.
        //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //    while (stateInfo.normalizedTime < 1f && stateInfo.length > 0)
        //    {
        //        yield return new WaitForFixedUpdate();
        //        stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        //    }
        //}

        if (extraWaitTime > 0f)
        {
            yield return new WaitForSeconds(extraWaitTime);
        }

        // Execute the configured trigger action.
        switch (triggerAction)
        {
            case HitBoxTriggerAction.Release:
                gameObject.Release();
                break;
            case HitBoxTriggerAction.Disable:
                gameObject.SetActive(false);
                break;
            case HitBoxTriggerAction.Destroy:
                Destroy(gameObject);
                break;
            case HitBoxTriggerAction.None:
            default:
                break;
        }
    }
}

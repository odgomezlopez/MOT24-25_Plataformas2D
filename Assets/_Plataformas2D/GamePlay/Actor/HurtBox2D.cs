using UnityEngine;
using UnityEngine.Events;

public class HurtBox2D : MonoBehaviour
{
    ActorController actorController;

    [SerializeField, Range(0f,5f)] float damagerModifier = 1f;
    [SerializeField] UnityEvent OnHurt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        actorController = GetComponentInParent<ActorController>();
        if (!actorController) Debug.LogError("The component HurtBox2D requieres to be child of a ActorController");
    }
    
    public void TakeDamage(float dmg,GameObject org)
    {
        actorController.TakeDamage(dmg*damagerModifier, org);
        OnHurt.Invoke();
    }
}

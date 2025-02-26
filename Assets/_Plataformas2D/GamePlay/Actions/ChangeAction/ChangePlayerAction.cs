using UnityEngine;

public class ChangePlayerAttack : MonoBehaviour
{
    public Action newAction;

    public enum ActionNumer { Action1, Action2 }
    public ActionNumer actionNumer = ActionNumer.Action1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!newAction) return;

        var playerController = collision.GetComponentInParent<PlayerController>();

        if (playerController)
        {
            if (actionNumer == ActionNumer.Action1) ((PlayerStats)playerController.Stats).action1 = newAction;
            if (actionNumer == ActionNumer.Action2) ((PlayerStats)playerController.Stats).action2 = newAction;
            Destroy(gameObject);
        }
    }

}

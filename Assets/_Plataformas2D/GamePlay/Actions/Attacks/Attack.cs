using System;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "new Attack", menuName = "Actions/Attack/AttackAnimt", order = 1)]
public class Attack : Action
{
    [Header("Attack Data")]
    public float damage = 1f;
    public string animationTrigger=""; //Use a trigger to trigger an animation

    //Sobreescribimos el método use
    public override void Use(GameObject g)
    {
        base.Use(g);

        if(animationTrigger != "")
        {
            var ac = g.GetComponentInChildren<Animator>();
            if (ac)
            {
                try
                {
                    ac.SetTrigger(animationTrigger);
                }
                catch(Exception e)
                {
                    Debug.LogError($"{g.name}'s animator does not have the {animationTrigger} trigger. Expcetion trace: {e}");
                }
            }
        }
    }
}

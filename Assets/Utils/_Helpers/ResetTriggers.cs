using System.Collections.Generic;
using UnityEngine;

public class ResetTriggers : StateMachineBehaviour
{
    // List to store trigger names
    private List<string> triggerNames = new List<string>();

    // This method is called when the state machine evaluates this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If the trigger list hasn't been populated yet, populate it
        if (triggerNames.Count == 0)
        {
            PopulateTriggerNames(animator);
        }

        // Reset all triggers in the list
        foreach (string triggerName in triggerNames)
        {
            animator.ResetTrigger(triggerName);
        }
    }

    // Populate the list with all trigger names from the Animator's parameters
    //private void PopulateTriggerNames(Animator animator)
    //{
    //    // Check if the Animator is using an AnimatorController
    //    if (animator.runtimeAnimatorController != null)
    //    {
    //        // Get the AnimatorController
    //        var animatorController = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

    //        if (animatorController != null)
    //        {
    //            // Loop through the parameters in the AnimatorController
    //            foreach (var parameter in animatorController.parameters)
    //            {
    //                if (parameter.type == AnimatorControllerParameterType.Trigger)
    //                {
    //                    // Add only triggers to the list
    //                    triggerNames.Add(parameter.name);
    //                }
    //            }
    //        }
    //    }
    //}

    private void PopulateTriggerNames(Animator animator)
    {
        // Check if the Animator has any parameters
        if (animator.parameters != null)
        {
            // Loop through the parameters of the Animator
            foreach (var parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    // Add only triggers to the list
                    triggerNames.Add(parameter.name);
                }
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTriggerEvent : MonoBehaviour
{
    [SerializeField] private string[] actorTags = { "Player" };

    public UnityEvent OnTagEnter;
    public UnityEvent OnTagExit;

    private void CheckTrigger(string tag, bool isEnter)
    {
        // Only proceed if the object has a valid tag from actorTags.
        if (!IsValidActorTag(tag)) return;

        if (isEnter) OnTagEnter.Invoke();
        else OnTagExit.Invoke();
    }

    private bool IsValidActorTag(string tag)
    {
        foreach (var validTag in actorTags)
        {
            if (tag.Equals(validTag))
                return true;
        }
        return false;
    }

    // 3D triggers
    private void OnTriggerEnter(Collider other) => CheckTrigger(other.tag, true);
    private void OnTriggerExit(Collider other) => CheckTrigger(other.tag, false);

    // 2D triggers
    private void OnTriggerEnter2D(Collider2D collision) => CheckTrigger(collision.tag, true);
    private void OnTriggerExit2D(Collider2D collision) => CheckTrigger(collision.tag, false);
}
using UnityEngine;
using UnityEngine.Events;

public class DistanceTrigger : MonoBehaviour
{
    [Header("Tracked Object")]
    [Tooltip("Object to track. If left empty, will attempt to find one by 'trackTag'.")]
    public GameObject tracked;

    [Tooltip("Fallback tag if 'tracked' is not assigned in Inspector.")]
    public string trackTag = "Player";

    [Header("Trigger Settings")]
    [Tooltip("Distance required to invoke OnTagEnter and OnTagExit.")]
    public float distanceThreshold = 5f;

    [Header("Trigger State")]
    [Tooltip("True if the tracked object is currently within distanceThreshold.")]
    public bool tagInArea;

    [Header("Events")]
    public UnityEvent OnTagEnter; // Invoked once on entering the distance range
    public UnityEvent OnTagExit;  // Invoked once on exiting the distance range

    private void Awake()
    {
        // If 'tracked' wasn’t assigned, try to find by tag.
        if (!tracked)
            tracked = GameObject.FindGameObjectWithTag(trackTag);

        // Initialize outside of the trigger range.
        tagInArea = false;
    }

    private void Update()
    {
        if (!tracked) return;

        // Measure distance to see if within threshold
        float distance = Vector3.Distance(transform.position, tracked.transform.position);

        // Enter range
        if (distance <= distanceThreshold)
        {
            if (!tagInArea)
            {
                tagInArea = true;
                OnTagEnter?.Invoke();
            }
        }
        // Exit range
        else
        {
            if (tagInArea)
            {
                tagInArea = false;
                OnTagExit?.Invoke();
            }
        }
    }

    // Draw a wire-sphere in the Scene view to visualize the distance trigger
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distanceThreshold);
    }
}

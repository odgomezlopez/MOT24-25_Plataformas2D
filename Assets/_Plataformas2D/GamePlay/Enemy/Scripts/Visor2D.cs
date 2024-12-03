// File: Scripts/Visor2D.cs
using UnityEngine;
using System.Linq;
using System;

public class Visor2D : MonoBehaviour
{
    // ======= Public Fields =======
    [Tooltip("Event triggered when the target's visibility changes")]
    public event Action<bool, Vector3> OnTargetSeen;

    [Tooltip("Tag of the target object to detect")]
    public string targetTag = "Player";

    [Tooltip("Horizontal distance for vision detection")]
    public float viewDistanceX = 5f;

    [Tooltip("Vertical distance for vision detection")]
    public float viewDistanceY =5f;

    [Tooltip("Detection radius in degrees")]
    public float detectRadius = 45f;

    [Tooltip("Invert the X-axis for vision points")]
    public bool invertAxis;

    [SerializeField, Range(1, 10)] private int frameRate = 1;

    // ======= Private Fields =======
    private GameObject target;
    private Collider2D targetCollider;
    private Vector3[] localVisionPoints;
    private bool visible;
    private Vector3 lastSeenTargetPosition;

    // ======= Unity Methods =======
    void Start()
    {
        Initialize();
        PrecomputeLocalVisionPoints();
    }

    void FixedUpdate()
    {
        if (Time.frameCount % frameRate == 0)
        {
            // Transform local points to world space for the current frame
            Vector3[] visionPoints = TransformLocalPointsToWorld();

            // Check target visibility
            bool newVisible = CheckTargetVisibility(GetTargetBounds(), visionPoints);

            if (newVisible != visible)
            {
                visible = newVisible;
                OnTargetSeen?.Invoke(visible, target != null ? target.transform.position : Vector3.zero);
            }

            else if (newVisible)
            {
                lastSeenTargetPosition = target.transform.position;
                OnTargetSeen?.Invoke(visible, target != null ? target.transform.position : Vector3.zero);

            }
        }
    }

    void OnDrawGizmos()
    {
        if (localVisionPoints == null) return;
        DrawVisionGizmos(TransformLocalPointsToWorld());
    }

    private void OnValidate()
    {
        PrecomputeLocalVisionPoints();
    }

    // ======= Initialization =======
    private void Initialize()
    {
        visible = false;
        lastSeenTargetPosition = Vector2.zero;
        FindTarget();
    }

    private void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        if (target != null)
        {
            targetCollider = target.GetComponentInChildren<Collider2D>();
        }
        else
        {
            Debug.LogWarning($"Target with tag '{targetTag}' not found in the scene!");
        }
    }

    private void PrecomputeLocalVisionPoints()
    {
        float angle = detectRadius / 2;
        float axisMultiplier = invertAxis ? 1 : -1;

        localVisionPoints = new[]
        {
            new Vector3(axisMultiplier * viewDistanceX, PosToAnguloY(angle) * viewDistanceY, 0),
            new Vector3(axisMultiplier * viewDistanceX, PosToAnguloY(-angle) * viewDistanceY, 0),
            new Vector3(axisMultiplier * PosToAnguloX(angle) * viewDistanceX, viewDistanceY, 0),
            new Vector3(axisMultiplier * PosToAnguloX(-angle) * viewDistanceX, -viewDistanceY, 0)
        };

        if (detectRadius > 90)
        {
            localVisionPoints[0].y = viewDistanceY;
            localVisionPoints[1].y = -viewDistanceY;
        }
    }

    // ======= Vision Points Calculation =======
    private Vector3[] TransformLocalPointsToWorld()
    {
        return localVisionPoints.Select(point => transform.localToWorldMatrix.MultiplyPoint(point)).ToArray();
    }

    // ======= Target Detection Logic =======
    private Vector3[] GetTargetBounds()
    {
        if (targetCollider == null) return Array.Empty<Vector3>();

        var center = targetCollider.bounds.center;
        var extents = targetCollider.bounds.extents;

        return new[]
        {
            target.transform.position,
            new Vector3(target.transform.position.x, center.y + extents.y, target.transform.position.z),
            new Vector3(target.transform.position.x, center.y - extents.y, target.transform.position.z)
        };
    }

    private bool CheckTargetVisibility(Vector3[] targetBounds, Vector3[] visionPoints)
    {
        if (targetBounds.Length == 0 || visionPoints.Length == 0) return false;

        return targetBounds.Any(destination =>
            IsPointVisible(destination, visionPoints) && CheckLineOfSight(destination));
    }

    private bool IsPointVisible(Vector3 point, Vector3[] visionPoints)
    {
        return PointInsideTrigon(point, transform.position, visionPoints[0], visionPoints[1]) ||
               (detectRadius > 90 && (PointInsideTrigon(point, transform.position, visionPoints[0], visionPoints[2]) ||
                                      PointInsideTrigon(point, transform.position, visionPoints[1], visionPoints[3]))) ||
               (detectRadius > 270 && PointInsideTrigon(point, transform.position, visionPoints[2], visionPoints[3]));
    }

    private bool CheckLineOfSight(Vector3 destination)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, destination - transform.position);
        return hit.collider != null && hit.collider.CompareTag(targetTag);
    }

    // ======= Vision Geometry Calculations =======
    private bool PointInsideTrigon(Vector3 s, Vector3 a, Vector3 b, Vector3 c)
    {
        float as_x = s.x - a.x;
        float as_y = s.y - a.y;

        bool s_ab = (b.x - a.x) * as_y - (b.y - a.y) * as_x > 0;

        if ((c.x - a.x) * as_y - (c.y - a.y) * as_x > 0 == s_ab)
            return false;
        if ((c.x - b.x) * (s.y - b.y) - (c.y - b.y) * (s.x - b.x) > 0 != s_ab)
            return false;
        return true;
    }

    private float PosToAnguloX(float angle)
    {
        return Mathf.Cos(ConvertDegreesToRadians(angle));
    }

    private float PosToAnguloY(float angle)
    {
        return Mathf.Sin(ConvertDegreesToRadians(angle));
    }

    private float ConvertDegreesToRadians(float degrees)
    {
        return Mathf.Deg2Rad * degrees;
    }

    // ======= Gizmos Drawing =======
    private void DrawVisionGizmos(Vector3[] visionPoints)
    {
        // Set the Gizmos color based on visibility
        Gizmos.color = visible ? Color.red : Color.yellow;

        // Ensure the matrix transformation is set to the world space
        Gizmos.matrix = Matrix4x4.identity;

        // Draw lines representing the vision area
        Gizmos.DrawLine(transform.position, visionPoints[0]);
        Gizmos.DrawLine(transform.position, visionPoints[1]);
        Gizmos.DrawLine(visionPoints[0], visionPoints[1]);

        if (detectRadius > 90)
        {
            Gizmos.DrawLine(transform.position, visionPoints[2]);
            Gizmos.DrawLine(transform.position, visionPoints[3]);
            Gizmos.DrawLine(visionPoints[0], visionPoints[2]);
            Gizmos.DrawLine(visionPoints[1], visionPoints[3]);

            if (detectRadius > 270)
                Gizmos.DrawLine(visionPoints[2], visionPoints[3]);
        }
    }
}

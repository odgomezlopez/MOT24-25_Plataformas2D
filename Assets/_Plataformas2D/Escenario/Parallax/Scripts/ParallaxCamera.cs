using UnityEngine;

[ExecuteInEditMode]
public class ParallaxCamera : MonoBehaviour
{
    public delegate void ParallaxCameraDelegate(Vector3 delta);
    public ParallaxCameraDelegate onCameraTranslate;

    private Vector3 oldPosition;

    void Start()
    {
        oldPosition = transform.position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, oldPosition) > 0)
        {
            if (onCameraTranslate != null)
            {
                Vector2 delta = oldPosition - transform.position;
                onCameraTranslate(delta);
            }

            oldPosition = transform.position;
        }
    }
}
using UnityEngine;

[ExecuteInEditMode]
public class ParallaxLayer : MonoBehaviour
{
    public Vector3 parallaxFactor;

    public void Move(Vector3 delta)
    {
        Vector3 newPos = transform.localPosition;
        newPos.x -= delta.x * parallaxFactor.x;
        newPos.y -= delta.y * parallaxFactor.y;

        transform.localPosition = newPos;
    }

}

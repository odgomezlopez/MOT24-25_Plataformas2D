using UnityEngine;

public class AnimationsResponses : MonoBehaviour
{
    public void OnEndDestroy()
    {
        Destroy(gameObject);
    }

    public void OnEndDestroyParent()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}

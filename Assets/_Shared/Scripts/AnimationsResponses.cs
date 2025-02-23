using UnityEngine;

public class AnimationsResponses : MonoBehaviour
{
    public void OnEndDestroy()
    {
        Destroy(gameObject);
    }

    public void OnEndDestroyParent()
    {
        Destroy(GetComponentInParent<HitBox2D>().gameObject);
        //Destroy(gameObject.transform.parent.gameObject);
    }

    public void OnEndPoolParent()
    {
        GetComponentInParent<HitBox2D>().gameObject.Release();
    }
}

using UnityEngine;

public class Action : ScriptableObject
{
    public string actionName = "";
    public float delay = 0f;

    public virtual void Use(GameObject g)
    {
        Debug.Log($"{g.name} has used {actionName}");
    }
}

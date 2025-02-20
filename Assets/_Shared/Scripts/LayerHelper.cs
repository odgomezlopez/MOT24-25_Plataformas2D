using UnityEngine;

public static class LayerHelper
{
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        // Set layer on the current object
        obj.layer = newLayer;

        // Recursively set layer on each child
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
using UnityEngine;

public static class PoolExtensions
{
    /// <summary>
    /// Spawns an object from the pool (or instantiates new) at a given world position/rotation.
    /// </summary>
    public static GameObject Spawn(this GameObject prefab, Vector3 position, Quaternion rotation, int layer = -1)
    {
        // If we have a valid PoolManager, use it
        if (ObjectPoolManager.Instance != null)
        {
            return ObjectPoolManager.Instance.Spawn(prefab, position, rotation,layer);
        }
        else
        {
            // Fallback to standard instantiation if no pool manager exists
            GameObject g = Object.Instantiate(prefab, position, rotation);
            if (layer != -1) LayerHelper.SetLayerRecursively(g, layer);
            return g;
        }
    }


    /// <summary>
    /// If you want a variant for local positioning instead of world.
    /// </summary>
    public static GameObject Spawn(this GameObject prefab,  Transform parent, int layer = -1)
    {
        // If we have a valid PoolManager, use it
        if (ObjectPoolManager.Instance != null)
        {
            return ObjectPoolManager.Instance.Spawn(prefab, parent, layer);
        }
        else
        {
            // Fallback to standard instantiation if no pool manager exists
            GameObject g = Object.Instantiate(prefab, parent);
            if (layer != -1) LayerHelper.SetLayerRecursively(g, layer);
            return g;
        }
    }

    /// <summary>
    /// Extension method to return an object to the pool or destroy it if no pool exists.
    /// </summary>
    public static void Release(this GameObject go)
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.Release(go);
        }
        else
        {
            Object.Destroy(go);
        }
    }

    /// <summary>
    /// Extension method to return an object to the pool or destroy it if no pool exists.
    /// </summary>
    public static void Release(this GameObject go,float delay)
    {
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.Release(go,delay);
        }
        else
        {
            Object.Destroy(go, delay);
        }
    }
}

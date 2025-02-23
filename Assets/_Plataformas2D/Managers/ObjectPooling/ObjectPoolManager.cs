using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using static UnityEditor.Progress;

[System.Serializable]
public class PoolItem
{
    public GameObject prefab;
    public int initialQuantity = 10;
}

public class ObjectPoolManager : MonoBehaviourSingleton<ObjectPoolManager>
{
    // === Singleton Setup ===


    // === Pool Definition ===
    [Header("Pre-populate pool with these items")]
    public List<PoolItem> pools = new List<PoolItem>();

    private Dictionary<GameObject, Queue<GameObject>> _poolDictionary
        = new Dictionary<GameObject, Queue<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
        // Prepopulate (pre-warm) the pool based on `poolItems`
        PrepopulatePools();
    }

    /// <summary>
    /// Pre-warm the pool by instantiating a given number of each prefab up front.
    /// </summary>
    private void PrepopulatePools()
    {
        foreach (var item in pools)
        {
            // Use prefab name as the key in dictionary
            var poolKey = item.prefab;

            if (!_poolDictionary.ContainsKey(poolKey))
            {
                _poolDictionary[poolKey] = new Queue<GameObject>();
            }

            // Instantiate and enqueue the requested number of copies
            for (int i = 0; i < item.initialQuantity; i++)
            {
                GameObject obj = Instantiate(item.prefab, transform);
                obj.name = item.prefab.name;  // keep consistent naming
                obj.SetActive(false);

                obj.AddComponent<PoolReference>();
                obj.GetComponent<PoolReference>().originalPrefab = item.prefab;

                _poolDictionary[poolKey].Enqueue(obj);
            }
        }
    }

    /// <summary>
    /// Get a pooled GameObject, placing it at the given world position/rotation (no parent).
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation, int layer = -1)
    {
        var poolKey = prefab;

        if (!_poolDictionary.ContainsKey(poolKey))
        {
            // Create an empty queue if none exists yet
            _poolDictionary[poolKey] = new Queue<GameObject>();
        }

        // If we already have something in the queue, grab it
        if (_poolDictionary[poolKey].Count > 0)
        {
            GameObject pooledObj = _poolDictionary[poolKey].Dequeue();
            pooledObj.transform.SetParent(null); // detach from manager
            pooledObj.transform.SetPositionAndRotation(position, rotation);
            if (layer != -1) LayerHelper.SetLayerRecursively(pooledObj, layer);
            pooledObj.SetActive(true);
            return pooledObj;
        }
        else
        {
            // Nothing available, so create a new one
            GameObject newObj = Instantiate(prefab, position, rotation);
            newObj.name = prefab.name;
            newObj.AddComponent<PoolReference>();
            newObj.GetComponent<PoolReference>().originalPrefab = prefab;

            if (layer != -1) LayerHelper.SetLayerRecursively(newObj, layer);
            return newObj;
        }
    }

    /// <summary>
    /// Overload to specify a parent transform.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Transform parent, int layer = -1)
    {
        var poolKey = prefab;

        if (!_poolDictionary.ContainsKey(poolKey))
        {
            _poolDictionary[poolKey] = new Queue<GameObject>();
        }

        if (_poolDictionary[poolKey].Count > 0)
        {
            GameObject pooledObj = _poolDictionary[poolKey].Dequeue();
            pooledObj.transform.SetParent(parent);
            pooledObj.transform.localPosition = Vector3.zero;
            pooledObj.transform.localRotation = Quaternion.identity;
            if(layer != -1) LayerHelper.SetLayerRecursively(pooledObj, layer);
            pooledObj.SetActive(true);

            return pooledObj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab,parent);
            newObj.name = prefab.name;
            newObj.AddComponent<PoolReference>();
            newObj.GetComponent<PoolReference>().originalPrefab = prefab;

            if (layer != -1) LayerHelper.SetLayerRecursively(newObj, layer);
            return newObj;
        }
    }

    /// <summary>
    /// Returns an object to the pool, disabling it.
    /// </summary>
    public void Release(GameObject obj)
    {

        obj.SetActive(false);
        obj.transform.SetParent(transform);

        var poolKey = obj.GetComponent<PoolReference>().originalPrefab ?? null;
        if (!_poolDictionary.ContainsKey(poolKey))
        {
            _poolDictionary[poolKey] = new Queue<GameObject>();
        }

        _poolDictionary[poolKey].Enqueue(obj);
    }

    /// <summary>
    /// Llama ReturnObject(obj) tras "delaySeconds" segundos.
    /// </summary>
    public void Release(GameObject obj, float delaySeconds)
    {
        StartCoroutine(ReleaseCoroutine(obj, delaySeconds));
    }

    private IEnumerator ReleaseCoroutine(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Release(obj);
    }
}

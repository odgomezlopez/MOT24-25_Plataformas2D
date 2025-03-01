using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class MonoBehaviourSingleton<T> : MonoBehaviour
	where T : Component
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this as T;
        }
    }
}


public class MonoBehaviourSingletonPersistent<T> : MonoBehaviour
	where T : Component
{
	public static T Instance { get; private set; }
	
	public virtual void Awake ()
	{
		if (Instance == null) {
			Instance = this as T;
			DontDestroyOnLoad (this);
		} else {
			Destroy (gameObject);
		}
	}
}

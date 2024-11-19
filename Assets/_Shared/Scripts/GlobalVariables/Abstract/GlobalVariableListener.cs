using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class GlobalVariableListener<T> : MonoBehaviour
{
    public GlobalVariable<T> gameEvent;
    public UnityEvent<T> onEventTriggered;

    void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventTriggered(T val)
    {
        onEventTriggered.Invoke(val);
    }
}

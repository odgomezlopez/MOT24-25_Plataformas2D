using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event/Float")]
public class GameEventFloat : ScriptableObject
{
    List<GameEventFloatListener> listeners = new List<GameEventFloatListener>();

    public void TriggerEvent(float val)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventTriggered(val);
    }

    public void AddListener(GameEventFloatListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventFloatListener listener)
    {
        listeners.Remove(listener);
    }
}
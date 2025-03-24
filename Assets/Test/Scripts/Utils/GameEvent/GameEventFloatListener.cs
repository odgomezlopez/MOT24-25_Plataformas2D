using UnityEngine;
using UnityEngine.Events;

public class GameEventFloatListener : MonoBehaviour
{
    public GameEventFloat gameEvent;
    public UnityEvent<float> onEventTriggered;

    void OnEnable()
    {
        gameEvent.AddListener(this);
    }

    void OnDisable()
    {
        gameEvent.RemoveListener(this);
    }

    public void OnEventTriggered(float val)
    {
        onEventTriggered.Invoke(val);
    }
}
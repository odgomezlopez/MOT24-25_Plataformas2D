using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[System.Serializable]
public class ScoreEvent
{
    public int requiredScore = 1;
    public bool triggered = false;
    public UnityEvent OnScore;
}

public class ScoreManager : MonoBehaviourSingleton<ScoreManager>
{
    [SerializeField] SmartVariable<int> score;
    [SerializeField] List<ScoreEvent> scoreEvents;


    void Start()
    {
        score.CurrentValue = 0;
        score.OnValueUpdate.Invoke(0);
    }

    public void AddScore(int inc)
    {
        score.CurrentValue += inc;

        //Compruebo la lista de eventos y lanzo los que se pueda
        foreach(ScoreEvent s in scoreEvents)
        {
            if(!s.triggered && score.CurrentValue >= s.requiredScore)
            {
                s.triggered = true;
                s.OnScore.Invoke();
            }
        }
    }
}

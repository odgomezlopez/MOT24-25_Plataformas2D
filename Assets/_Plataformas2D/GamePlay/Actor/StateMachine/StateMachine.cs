using UnityEngine;

[System.Serializable]
public class StateMachine
{
    public IState currentState;
    [SerializeField] string currentStateName = "";

    // Update is called once per frame
    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newState;
        currentState.OnEnter();
        currentStateName = currentState.ToString();
    }
}

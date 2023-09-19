using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State currentState { get; set; }

    public void Initialize(State startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void ChangeState(State newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}

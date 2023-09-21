using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State currentState { get; set; }
    public NPC_State currentStateNPC { get; set; }

    public void Initialize(State startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void InitializeNPC(NPC_State startingState)
    {
        currentStateNPC = startingState;
        currentStateNPC.EnterState();
    }

    public void ChangeState(State newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    public void ChangeStateNPC(NPC_State newState)
    {
        currentStateNPC.ExitState();
        currentStateNPC = newState;
        currentStateNPC.EnterState();
    }
}

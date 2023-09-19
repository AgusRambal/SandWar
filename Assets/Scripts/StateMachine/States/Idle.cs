using UnityEngine;

public class Idle : State
{
    public Idle(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    { 
    }

    public override void EnterState()
    {
        Debug.Log("Estoy en idle");
    }

    public override void Update()
    {
        if (marine.isMoving)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }
}

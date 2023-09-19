using UnityEngine;

public class Idle : State
{
    public Idle(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    { 
    }

    public override void EnterState()
    {
    }

    public override void Update()
    {
        marine.animator.SetFloat("Velocity", marine.agent.remainingDistance);

        if (marine.isMoving)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }
}

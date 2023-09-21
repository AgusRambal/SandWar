using UnityEngine;

public class Idle : State
{
    public Idle(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    { 
    }

    public override void EnterState()
    {
        marine.isMoving = false;
    }

    public override void Update()
    {
        marine.animator.SetFloat("Velocity", 0);

        if (marine.isMoving)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }
}

using UnityEngine;

public class Idle : State
{
    public Idle(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    { 
    }

    public override void EnterState()
    {
        marine.animator.SetBool("isWalking", false);
    }

    public override void Update()
    {
        if (marine.isMoving)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }
}

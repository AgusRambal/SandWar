using UnityEngine;

public class Walking : State
{
    public Walking(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.isMoving = true;
    }

    public override void Update()
    {
        marine.animator.SetFloat("Velocity", marine.agent.remainingDistance);

        if (!marine.isMoving)
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }
    }
}

using UnityEngine;
using UnityEngine.AI;

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
        marine.animator.SetFloat("Velocity", 0);

        if (!marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }
}

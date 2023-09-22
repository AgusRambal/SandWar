using UnityEngine;
using UnityEngine.AI;

public class Walking : State
{
    public Walking(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
    }

    public override void Update()
    {
        marine.animator.SetFloat("Velocity", marine.agent.remainingDistance);

        if (marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }
    }
}

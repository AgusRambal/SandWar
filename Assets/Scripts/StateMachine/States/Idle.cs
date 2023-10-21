using Core.Characters;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Idle(Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    { 
    }

    public override void EnterState()
    {
        marine.animator.SetBool("isShooting", false);
    }

    public override void Update()
    {
        MovingCheck();
        GoToAttack();
    }

    private void MovingCheck()
    {
        marine.animator.SetFloat("Velocity", 0);

        if (!marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }

    private void GoToAttack()
    { 
        //Checks if an enemy is in a certain radius and start attacking
    }
}

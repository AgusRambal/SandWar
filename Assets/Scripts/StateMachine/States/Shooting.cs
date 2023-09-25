using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : State
{
    public Shooting(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.animator.SetBool("isShooting", true);
        marine.animator.SetTrigger("shoot");
    }

    public override void Update()
    {
        MovingCheck();
    }

    private void MovingCheck()
    {
        if (!marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : State
{
    private float time;

    public Shooting(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        ShootAnim();
    }

    public override void Update()
    {
        MovingCheck();
    }

    private void MovingCheck()
    {
        marine.animator.SetFloat("Velocity", 0);

        if (!marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }

    private void ShootAnim()
    {
        marine.animator.SetBool("isShooting", true);
    }
}

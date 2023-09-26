using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : State
{
    public Shooting(Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.actualWeapon.IsShooting = true;
        marine.actualWeapon.Shoot(marine.TotalAccuracy, marine.animator);
    }

    public override void Update()
    {
        MovingCheck();
        FinishState();
    }

    private void MovingCheck()
    {
        if (!marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }

    private void FinishState()
    { 
        if (marine.actualWeapon.targetKilled) 
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }
    }
}

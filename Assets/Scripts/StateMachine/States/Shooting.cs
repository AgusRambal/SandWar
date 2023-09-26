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
        if (marine.actualWeapon.isShooting)
            return;

        marine.actualWeapon.Shoot(marine.totalAccuracy, marine.animator);
        marine.actualWeapon.isShooting = true;
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

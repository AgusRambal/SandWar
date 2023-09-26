using UnityEngine;

public class Shooting : State
{
    private float timer = 0;

    public Shooting(Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.actualWeapon.targetKilled = false;
    }

    public override void Update()
    {
        MovingCheck();
        StopShooting();
        ShootingState();
    }

    private void MovingCheck()
    {
        if (!marine.agent.isStopped)
        {
            marine.StateMachine.ChangeState(marine.WalkingState);
        }
    }

    private void ShootingState()
    {
        timer += Time.deltaTime;

        if (timer >= marine.actualWeapon.Weapon.FireRate)
        {
            marine.actualWeapon.Shoot(marine.TotalAccuracy, marine.animator);
            timer = 0;
        }
    }

    private void StopShooting()
    {
        if (marine.actualWeapon.targetKilled)
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }

        if (marine.actualWeapon.bulletsLeft <= 0 && marine.magazines <= 0)
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }

        else if (marine.actualWeapon.bulletsLeft <= 0 && marine.magazines > 0)
        {
            marine.StateMachine.ChangeState(marine.ReloadingState);
        }
    }
}

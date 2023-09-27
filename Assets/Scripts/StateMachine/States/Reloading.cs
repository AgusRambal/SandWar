using UnityEngine;

public class Reloading : State
{
    private float timer = 0;

    public Reloading(Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
    }

    public override void Update()
    {
        Reload();
    }

    public void Reload()
    {
        timer += Time.deltaTime;
        //Start animation

        if (timer >= marine.actualWeapon.Weapon.ReloadTime) //El reload time deberia ser la duracion de la animcion + un poquito
        {
            timer = 0;
            marine._stats.Magazines--;
            marine.actualWeapon.bulletsLeft = marine.actualWeapon.Weapon.BulletsOnMagazine;
            marine.StateMachine.ChangeState(marine.ShootingState);
        }
    }
}

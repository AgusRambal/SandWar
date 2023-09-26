using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Variables")]
    public Weapon weapon;
    public Transform weaponPivot;
    public bool isShooting;
    public LayerMask enemyLayer;

    protected int bulletsLeft;
    protected int bulletsShot;
    protected bool readyToShoot;
    protected bool reloading;

    protected Weapon Weapon => weapon;
    protected Transform WeaponPivot => weaponPivot;

    public virtual void Start()
    {
        bulletsLeft = Weapon.BulletsOnMagazine;
    }

    public virtual void Update()
    {
    }

    public void Shoot(float accuracy, Animator animator)
    {
        readyToShoot = false;

        animator.SetBool("isShooting", true);
        animator.SetTrigger("shoot");
        //Activar el muzzle flash
        //Reproducir el sonido de disparo

        if (Physics.Raycast(WeaponPivot.position, WeaponPivot.transform.forward, out RaycastHit hit, enemyLayer))
        {
            if (hit.transform.GetComponent<Insurgent>())
            {
                var enemy = hit.transform.GetComponent<Insurgent>();

                var possibility = Random.Range(0, 100);

                if (accuracy >= possibility)
                {
                    enemy.health -= 10;
                    Debug.Log(enemy.health);
                }

                else
                {
                    Debug.Log("erre");
                }
            }
        }

        bulletsLeft--;

        StartCoroutine(ResetShot(accuracy, animator)); //Time between shooting
    }

    private IEnumerator ResetShot(float accuracy, Animator animator)
    {
        yield return new WaitForSeconds(Weapon.FireRate);
        readyToShoot = true;
        Shoot(accuracy, animator);
    }
}

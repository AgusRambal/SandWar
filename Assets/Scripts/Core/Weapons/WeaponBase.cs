using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private LayerMask enemyLayer;
    public bool IsShooting;
    public bool targetKilled = false;

    protected int bulletsLeft;
    protected int bulletsShot;
    protected bool reloading;

    public Weapon Weapon => weapon;
    public Transform WeaponPivot => weaponPivot;
    public LayerMask EnemyLayer => enemyLayer;

    public virtual void Start()
    {
        bulletsLeft = Weapon.BulletsOnMagazine;
    }

    public virtual void Update()
    {
    }

    public void Shoot(float accuracy, Animator animator)
    {
        if (!IsShooting)
            return;

        if (reloading)
            return;

        if (bulletsLeft < 1)
        {
            StartCoroutine(Reload(accuracy, animator));
            reloading = true;
        }

        targetKilled = false;
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
                    if (enemy.health <= 0)
                    {
                        Debug.Log("listo");
                        IsShooting = false;
                        targetKilled = true;
                    }

                    enemy.health -= 10;
                }

                else
                {
                    //Debug.Log("erre");
                }
            }
        }

        bulletsLeft--;

        Debug.Log(bulletsLeft);

        StartCoroutine(ResetShot(accuracy, animator)); //Time between shooting
    }

    private IEnumerator ResetShot(float accuracy, Animator animator)
    {
        yield return new WaitForSeconds(Weapon.FireRate);
        Shoot(accuracy, animator);
    }

    private IEnumerator Reload(float accuracy, Animator animator)
    {
        yield return new WaitForSeconds(Weapon.ReloadTime); //El reload time deberia ser la duracion de la animcion + un poquito
        //Reproducir animacion
        bulletsLeft = Weapon.BulletsOnMagazine;
        reloading = false;
        StartCoroutine(ResetShot(accuracy, animator));
    }
}

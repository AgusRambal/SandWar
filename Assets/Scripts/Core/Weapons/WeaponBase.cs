using System.Collections;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private LayerMask enemyLayer;

    public bool targetKilled { get; set; }
    public int bulletsLeft { get; set; }

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

                //Pego el tiro
                if (accuracy >= possibility)
                {
                    //Mato al enemigo
                    if (enemy.CurrentHealth <= 0)
                    {
                        targetKilled = true;
                    }

                    EventManager.TriggerEvent(GenericEvents.ApplyDamageToEnemy, new Hashtable() {
                    {GameplayEventHashtableParams.DamageAmount.ToString(), weapon.Damage}
                    });
                }
            }
        }

        bulletsLeft--;
    }
}

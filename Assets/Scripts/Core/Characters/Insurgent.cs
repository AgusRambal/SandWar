using UnityEngine;

public class Insurgent : MonoBehaviour
{
    //Datos dummy para testear el sistema de disparo
    [SerializeField] private Character insurgent;
    [field: SerializeField] public float CurrentHealth { get; set; }
    public float MaxHealth { get; private set; }

    private void Awake()
    {
        MaxHealth = insurgent.MaxHealth;
        CurrentHealth = MaxHealth;
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        //No hacer destroy, dejar el cuerpo ahi tirado en el piso
        Destroy(gameObject);
    }
}

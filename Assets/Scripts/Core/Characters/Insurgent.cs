using System.Collections;
using UnityEngine;

public class Insurgent : MonoBehaviour, IEventListener
{
    //Datos dummy para testear el sistema de disparo
    [SerializeField] private Character insurgent;
    [field: SerializeField] public float CurrentHealth { get; set; }
    public float MaxHealth { get; private set; }

    private void Awake()
    {
        OnEnableEventListenerSubscriptions();
        MaxHealth = insurgent.MaxHealth;
        CurrentHealth = MaxHealth;
    }

    public void ApplyDamageToEnemy(Hashtable hashtable)
    {
        float damageAmount = (float)hashtable[GameplayEventHashtableParams.DamageAmount.ToString()];

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

    public void OnEnableEventListenerSubscriptions()
    {
        EventManager.StartListening(GenericEvents.ApplyDamageToEnemy, ApplyDamageToEnemy);
    }

    public void CancelEventListenerSubscriptions()
    {
        EventManager.StopListening(GenericEvents.ApplyDamageToEnemy, ApplyDamageToEnemy);
    }

    private void OnDisable()
    {
        CancelEventListenerSubscriptions();
    }

    private void OnDestroy()
    {
        CancelEventListenerSubscriptions();

    }
}

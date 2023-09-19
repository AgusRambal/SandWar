using UnityEngine;
using UnityEngine.AI;

public class MarineObject : MonoBehaviour, IDamageable
{
    [Header("References")]
    public Marine scirptableObject;
    public NavMeshAgent agent;
    public Animator animator;
    public AnimatorOverrideController animatorOverride;

    [Header("Stats")]
    [SerializeField] private int id;
    [SerializeField] private float health;
    public TypeMarine typeMarine;
    [SerializeField] private Weapon weapon;
    public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }

    //State machine
    public StateMachine StateMachine { get; set; }
    public Idle IdleState { get; set; }
    public Walking WalkingState { get; set; }

    //Flags
    [HideInInspector] public bool isMoving;

    private void Awake()
    {
        StateMachine = new StateMachine();
        IdleState = new Idle(this, StateMachine);
        WalkingState = new Walking(this, StateMachine);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = animatorOverride;
        typeMarine = scirptableObject.TypeMarine;
        health = scirptableObject.Health;
        id = scirptableObject.Id;
        weapon = scirptableObject.Weapon;
        MaxHealth = health;
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        StateMachine.currentState.Update();

        if (agent.remainingDistance <= 1f)
        {
            isMoving = false;
        }
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
        Destroy(gameObject);
    }
}

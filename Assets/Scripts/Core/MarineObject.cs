using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class MarineObject : MonoBehaviour, IDamageable
{
    [Header("References")]
    public Marine scirptableObject;
    public NavMeshAgent agent;
    public Animator animator;
    public AnimatorOverrideController animatorOverride;
    [SerializeField] private GameObject selectionArrow;
    public SelectableCharacter mySelf;

    [Header("Stats")]
    public int id;
    [SerializeField] private float health;
    public TypeMarine typeMarine;
    [SerializeField] private Weapon weapon;
    public float MaxHealth { get; set; }
    [field: SerializeField] public float CurrentHealth { get; set; }

    [Header("Battle")]
    public Insurgent target;

    //State machine
    public StateMachine StateMachine { get; set; }
    public Idle IdleState { get; set; }
    public Walking WalkingState { get; set; }
    public Shooting ShootingState { get; set; }

    private void Awake()
    {
        StateMachine = gameObject.AddComponent<StateMachine>();
        IdleState = new Idle(this, StateMachine);
        WalkingState = new Walking(this, StateMachine);
        ShootingState = new Shooting(this, StateMachine);

        DOTween.Init();

        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorOverride;
        typeMarine = scirptableObject.TypeMarine;
        health = scirptableObject.Health;
        id = scirptableObject.Id;
        weapon = scirptableObject.Weapon;
        MaxHealth = health;
        SelectionManager.Instance.AvailableMarines.Add(this);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        StateMachine.currentState.Update();
    }

    public void MoveTo(Vector3 position)
    { 
        agent.SetDestination(position);
    }

    public void OnSelected()
    {
        selectionArrow.transform.DOScale(1.5f, .2f);
    }

    public void OnDeselected()
    {
        selectionArrow.transform.DOScale(0f, .2f);
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
        //Quitar de la lista de myMairnes
        Destroy(gameObject);
    }
}

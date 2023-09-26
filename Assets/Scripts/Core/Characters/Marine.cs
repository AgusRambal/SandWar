using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Marine : MonoBehaviour, IDamageable
{
    [Header("References")]
    public Character character;
    public NavMeshAgent agent;
    public Animator animator;
    public AnimatorOverrideController animatorOverride;
    [SerializeField] private GameObject selectionArrow;
    public SelectableCharacter mySelf;
    public CustomLookAtTarget customLookAtTarget;
    public WeaponBase actualWeapon;

    [Header("Stats")]
    [SerializeField] private Weapon weapon;
    public float CurrentHealth { get; set; }
    public float totalAccuracy { get; private set; }

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
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorOverride;
        SelectionManager.Instance.AvailableMarines.Add(this);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);
        CurrentHealth = character.Health;
        totalAccuracy = character.Accuracy + actualWeapon.weapon.Accuracy; ;
    }

    private void Update()
    {
        StateMachine.currentState.Update();
    }

    public void MoveTo(Vector3 position)
    {
        agent.isStopped = false;
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

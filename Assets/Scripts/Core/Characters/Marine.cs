using DG.Tweening;
using System.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class Marine : MonoBehaviour
{
    [Header("References")]
    public Character character;
    public NavMeshAgent agent;
    public Animator animator;
    public AnimatorOverrideController animatorOverride;
    public SelectableCharacter characterSelectionImage;
    public WeaponBase actualWeapon;
    [SerializeField] private GameObject selectionArrow;

    [Header("Battle")]
    public Insurgent target;

    //State machine
    public StateMachine StateMachine { get; set; }
    public Idle IdleState { get; set; }
    public Walking WalkingState { get; set; }
    public Shooting ShootingState { get; set; }
    public Reloading ReloadingState { get; set; }

    [Header("LookAt")]
    [SerializeField] private float speed = 1f;
    public Coroutine LookCoroutine { get; private set; }

    public struct Stats
    {
        public float CurrentHealth;
        public float TotalAccuracy;
        public int Magazines;
    }

    private Stats _stats;

    public Stats Statss { get; set; }


    private void Awake()
    {
        StateMachine = gameObject.AddComponent<StateMachine>();
        IdleState = new Idle(this, StateMachine);
        WalkingState = new Walking(this, StateMachine);
        ShootingState = new Shooting(this, StateMachine);
        ReloadingState = new Reloading(this, StateMachine);

        DOTween.Init();

        animator.runtimeAnimatorController = animatorOverride;
        SelectionManager.Instance.AvailableMarines.Add(this);
    }

    private void Start()
    {
        StateMachine.Initialize(IdleState);

        _stats = new Stats
        {
            CurrentHealth = character.MaxHealth,
            TotalAccuracy = character.Accuracy + actualWeapon.Weapon.Accuracy,
            Magazines = character.Magazines
        };
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
        _stats.CurrentHealth -= damageAmount;

        if (_stats.CurrentHealth <= 0)
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

    public void StartRotating(Transform target)
    {
        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(target));
    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);
            time += Time.deltaTime * speed;
            yield return null;
        }
    }
}

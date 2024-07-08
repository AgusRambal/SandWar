using System.Collections;
using DG.Tweening;
using Interfaces;
using Transports;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using VehicleSystem.Transports;

namespace Core.Characters
{
    public class Marine : MonoBehaviour, IUnit, IEventListener
    {
        [FormerlySerializedAs("character")]
        [Header("References")]
        public Character characterScriptableObject;
        public NavMeshAgent agent;
        public Animator animator;
        public AnimatorOverrideController animatorOverride;
        public SelectableCharacter characterSelectionImage;
        public WeaponBase actualWeapon;
        [SerializeField] private GameObject selectionArrow;

        [Header("Battle")]
        public Insurgent target;

        // State machine
        public StateMachine StateMachine { get; set; }
        public Idle IdleState { get; set; }
        public Walking WalkingState { get; set; }
        public Shooting ShootingState { get; set; }
        public Reloading ReloadingState { get; set; }

        [Header("LookAt")]
        [SerializeField] private float speed = 1f;
        public Coroutine LookCoroutine { get; private set; }

        private ITransport currentTransport;
        protected ITransport CurrentTransport => currentTransport;

        public struct Stats
        {
            public float CurrentHealth;
            public float TotalAccuracy;
            public int Magazines;
        }

        public Stats _stats;

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

            OnEnableEventListenerSubscriptions();
        }

        private void Start()
        {
            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            StateMachine.currentState.Update();
        }

        void OnAnimatorMove()
        {
            if (StateMachine.currentState is Walking walkingState)
            {
                walkingState.OnAnimatorMove();
            }
        }

        public void MoveTo(Vector3 position)
        {
            agent.stoppingDistance = .1f;
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
            // No hacer destroy, dejar el cuerpo ahi tirado en el piso
            // Quitar de la lista de myMairnes
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

        public void HandleInteraction(IInteractable interactable)
        {
            if (this.characterScriptableObject.SubType == SubType.Driver)
            {
                interactable.Interact(this); // Mandar evento
            }
            else
            {
                EventManager.TriggerEvent(GenericEvents.GetRandomPosition, new Hashtable()
                {
                    {GameplayEventHashtableParams.Agent.ToString(), this}
                });
                Debug.Log(this.characterScriptableObject.SubType); // Mandar evento para subirse a las posiciones de copilotos
            }
        }

        public void AssignVehicle(ITransport vehicle)
        {
            currentTransport = vehicle as Transport;
        }

        public object GetUnitName()
        {
            return gameObject.name;
        }

        private void GetDriverPosition(Hashtable obj)
        {
            Vector3 destination = (Vector3)obj[GameplayEventHashtableParams.GetDriverPosition.ToString()];
            Marine unit = (Marine)obj[GameplayEventHashtableParams.Agent.ToString()];

            if (unit == this)
            {
                StopAllCoroutines();
                agent.SetDestination(destination);
                StartCoroutine(CheckIfReachedDestination());
            }
        }

        private void SetRandomWaypoint(Hashtable obj)
        {
            Vector3 destination = (Vector3)obj[GameplayEventHashtableParams.GetRandomWaypoint.ToString()];
            Marine unit = (Marine)obj[GameplayEventHashtableParams.Agent.ToString()];

            if (unit == this)
            {
                StopAllCoroutines();
                agent.SetDestination(destination);
                StartCoroutine(CheckIfReachedDestination());
            }
        }

        private IEnumerator CheckIfReachedDestination()
        {
            while (true)
            {
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !agent.hasPath)
                {
                    EnterCar();
                    yield break;
                }
                yield return new WaitForSeconds(.1f);
            }
        }

        public virtual void EnterCar()
        {

        }

        public void OnEnableEventListenerSubscriptions()
        {
            EventManager.StartListening(GenericEvents.GetDriverPosition, GetDriverPosition);
            EventManager.StartListening(GenericEvents.SetRandomWaypoint, SetRandomWaypoint);
        }

        public void CancelEventListenerSubscriptions()
        {
            throw new System.NotImplementedException();
        }
    }
}

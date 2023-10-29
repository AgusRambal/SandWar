using System.Collections;
using System.Collections.Generic;
using Core.Characters;
using Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace VehicleSystem.Transports
{
    public enum BasicStates
    {
        Off,
        On,
        Idle,
        InMove
    }

    public struct BasicStats
    {
        public int maxCapacityInVehicle;
    }

    public abstract class Transport : MonoBehaviour , IEventListener , ITransport , IInteractable
    {
        [SerializeField] private VehicleSystem.Transports.Transports transport;
        [SerializeField] private BasicStates basicStates;
        [SerializeField] private NavMeshAgent navMeshAgent;
        [SerializeField] private Transform waypointOfDriverPosition;
        [SerializeField] private List<Transform>  waypoints = new List<Transform>();
       
        private IUnit driver;
        public List<IUnit> unitsInTransport = new List<IUnit>();

        #region Properties

        protected VehicleSystem.Transports.Transports Transports => transport;
        protected BasicStates BasicStates => basicStates;
        protected NavMeshAgent NavMeshAgent => navMeshAgent;
        protected Transform WaypointOfDriverPosition => waypointOfDriverPosition;
        protected IUnit Driver => driver;
        
        protected BasicStats basicStats;

        protected bool ImSelected;
        
        #endregion

        protected virtual void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            OnEnableEventListenerSubscriptions();
        }

        protected void Start()
        {
            basicStats = new BasicStats()
            {
                maxCapacityInVehicle = transport.MaxCapacityOfUnit
            };
        }

        private void TurnOnTransport(Hashtable obj)
        {
            Marine marine = (Marine)obj[GameplayEventHashtableParams.Agent.ToString()];
            Transport me = (Transport)obj[GameplayEventHashtableParams.Transport.ToString()];
            
            if (me == this)
            {
                driver = marine;
                basicStates = BasicStates.On;
            }
        }

        private void CommonUnitInCar(Hashtable obj)
        {
            Marine marine = (Marine)obj[GameplayEventHashtableParams.Agent.ToString()];
            Transport me = (Transport)obj[GameplayEventHashtableParams.Transport.ToString()];
            
            if (me == this)
            {
                basicStats.maxCapacityInVehicle--;
            }
        }

        private Vector3 GetDriverPosition()
        {
            return WaypointOfDriverPosition.position;
        }

        private Vector3 GetRandomWaypoint()
        {
            int randomIndex = Random.Range(0, waypoints.Count);
            return waypoints[randomIndex].position;
        }

        public void Interact(IUnit unit)
        {
            EventManager.TriggerEvent(GenericEvents.GetDriverPosition, new Hashtable()
            {
                { GameplayEventHashtableParams.GetDriverPosition.ToString(), GetDriverPosition() },
                { GameplayEventHashtableParams.Agent.ToString(), unit }
            });
        }

        private void InteractGetRandomWaypoint(Hashtable obj)
        {
            Marine unit = (Marine)obj[GameplayEventHashtableParams.Agent.ToString()];
            
            EventManager.TriggerEvent(GenericEvents.SetRandomWaypoint,new Hashtable()
            {
                {GameplayEventHashtableParams.GetRandomWaypoint.ToString() ,GetRandomWaypoint()},
                {GameplayEventHashtableParams.Agent.ToString(),unit}
            });
        }

        public void OnEnableEventListenerSubscriptions()
        {
            EventManager.StartListening(GenericEvents.UnitHasEnteredToTheTransport,TurnOnTransport);
            EventManager.StartListening(GenericEvents.GetRandomPosition,InteractGetRandomWaypoint);
            EventManager.StartListening(GenericEvents.CommonUnitHasEnteredToTheTransport,CommonUnitInCar);
        }

        public void CancelEventListenerSubscriptions()
        {
            EventManager.StartListening(GenericEvents.UnitHasEnteredToTheTransport,TurnOnTransport);
        }

        public IUnit CurrentDriver { get; set; }

        public void CanStartDrive()
        {
            ImSelected = true;
        }

        public void Deselect()
        {
            ImSelected = false;
        }
    }
}

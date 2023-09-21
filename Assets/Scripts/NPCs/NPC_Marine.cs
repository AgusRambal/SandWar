using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Marine : MonoBehaviour
{
    //State machine
    public StateMachine StateMachine { get; set; }
    public NPC_Idle IdleState { get; set; }
    public NPC_Patrolling PatrollingState { get; set; }

    public NavMeshAgent agent;
    public Animator animator;
    public AnimatorOverrideController animatorOverride;

    public Transform pathParent;
    public List<Transform> pathToPatroll = new List<Transform>();

    [HideInInspector] public bool isMoving;

    private void Awake()
    {
        StateMachine = new StateMachine();
        IdleState = new NPC_Idle(this, StateMachine);
        PatrollingState = new NPC_Patrolling(this, StateMachine);

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = animatorOverride;
    }

    private void Start()
    {
        StateMachine.InitializeNPC(IdleState);

        for (int i = 0; i < pathParent.childCount; i++)
        {
            pathToPatroll.Add(pathParent.GetChild(i));
        }
    }

    private void Update()
    {
        StateMachine.currentStateNPC.Update();
    }
}

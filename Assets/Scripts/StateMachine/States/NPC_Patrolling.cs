using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Patrolling : NPC_State
{
    private int actualPath = 0;

    public NPC_Patrolling(NPC_Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.isMoving = true;
        marine.agent.SetDestination(marine.pathToPatroll[0].position);
        actualPath++;
        marine.animator.SetFloat("Velocity", 2);
    }

    public override void Update()
    {
        if (actualPath == marine.pathToPatroll.Count)
        {
            actualPath = 0;
        }

        if (marine.agent.remainingDistance <= 1.5)
        {
            marine.agent.SetDestination(marine.pathToPatroll[actualPath].position);
            actualPath++;
            Debug.Log(actualPath);
        }
    }
}

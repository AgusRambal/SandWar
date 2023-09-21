using UnityEngine;

public class NPC_Idle : NPC_State
{

    private float timer = 0;
    private float time;

    public NPC_Idle(NPC_Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        time = Random.Range(3f, 7f);
    }

    public override void Update()
    {
        marine.animator.SetFloat("Velocity", 0);

        timer += Time.deltaTime;

        if (timer >= time) 
        {
            marine.StateMachine.ChangeStateNPC(marine.PatrollingState);
        }
    }
}

using UnityEngine;

public class Walking : State
{
    public Walking(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        Debug.Log("Me muevo");
    }

    public override void Update()
    {
        if (!marine.isMoving)
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }
    }
}

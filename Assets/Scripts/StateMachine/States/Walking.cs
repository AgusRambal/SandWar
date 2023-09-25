using UnityEngine;

public class Walking : State
{
    public Walking(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.animator.SetBool("isShooting", false);

        if (marine.customLookAtTarget.LookCoroutine == null)
            return;

        marine.customLookAtTarget.StopCoroutine(marine.customLookAtTarget.LookCoroutine);
    }

    public override void Update()
    {
        MovingCheck();
        HandleAttack();
    }

    private void MovingCheck()
    {
        marine.animator.SetFloat("Velocity", marine.agent.remainingDistance);

        if (marine.agent.isStopped && marine.target == null)
        {
            marine.StateMachine.ChangeState(marine.IdleState);
        }
    }

    private void HandleAttack()
    {
        if (marine.target == null)
            return;

        if (Vector3.Distance(marine.transform.position, marine.target.transform.position) <= 8f)
        {
            marine.agent.isStopped = true;
            marine.StateMachine.ChangeState(marine.ShootingState);
        }
    }
}

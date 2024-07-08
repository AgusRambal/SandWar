using Core.Characters;
using UnityEngine;

public class Walking : State
{
    private Vector2 smoothDeltaPosition;

    public Walking(Marine marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
        marine.animator.applyRootMotion = true;
        marine.agent.updatePosition = false;
        marine.agent.updateRotation = true;

        marine.animator.SetBool("isShooting", false);
        marine.animator.SetBool("Walk", true);

        if (marine.LookCoroutine != null)
        {
            marine.StopCoroutine(marine.LookCoroutine);
        }
    }

    public override void Update()
    {
        MovingCheck();
        HandleAttack();
    }

    private void MovingCheck()
    {
        Vector3 worldDeltaPosition = marine.agent.nextPosition - marine.transform.position;

        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(marine.transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(marine.transform.forward, worldDeltaPosition);

        Vector2 deltaPosition = new Vector2(dx, dy);
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        if (marine.agent.remainingDistance <= 0.6f && marine.target == null)
        {
            marine.agent.isStopped = true;
            marine.StateMachine.ChangeState(marine.IdleState);
        }    
    }

    private void HandleAttack()
    {
        if (marine.target == null)
            return;

        if (Vector3.Distance(marine.transform.position, marine.target.transform.position) <= marine.actualWeapon.Weapon.MaximumRangeAccuracy)
        {
            marine.agent.isStopped = true;
            marine.actualWeapon.targetKilled = false;
            marine.StateMachine.ChangeState(marine.ShootingState);
        }
    }

    public void OnAnimatorMove()
    {
        if (marine.animator)
        {
            Vector3 position = marine.animator.rootPosition;
            position.y = marine.agent.nextPosition.y;
            marine.transform.position = position;
            marine.agent.nextPosition = position;
        }
    }
}

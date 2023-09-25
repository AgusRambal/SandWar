using UnityEngine;
using UnityEngine.AI;

public class Walking : State
{
    public Walking(MarineObject marine, StateMachine stateMachine) : base(marine, stateMachine)
    {
    }

    public override void EnterState()
    {
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
        if (Input.GetMouseButtonDown(1))
        {
            marine.target = null;
        }

        if (Input.GetMouseButtonDown(1) && SelectionManager.Instance.SelectedMarines.Count > 0)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitEnemy))
            {
                marine.target = hitEnemy.transform.GetComponent<Insurgent>();
            }
        }

        if (marine.target == null)
            return;

        if (Vector3.Distance(marine.transform.position, marine.target.transform.position) <= 8f)
        {
            marine.agent.isStopped = true;
            marine.StateMachine.ChangeState(marine.ShootingState);
        }
    }
}

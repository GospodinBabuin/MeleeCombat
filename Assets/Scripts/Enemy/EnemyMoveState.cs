using UnityEngine;

public class EnemyMoveState : EnemyBaseState
{
    public EnemyMoveState(EnemóStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory)
    {
        IsRootState = false;
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (Vector3.Distance(Context.transform.position, Context.PlayerTransform.position) < Context.StopDistance && Context.CanAttack)
        {
            SwitchState(Factory.MeleeAttack());
        }
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        Context.NavMeshAgent.destination = Context.PlayerTransform.position;
        Context.Animator.SetFloat(Context.AnimIDSpeed, Context.NavMeshAgent.velocity.magnitude);

        CheckSwitchStates();
    }
}

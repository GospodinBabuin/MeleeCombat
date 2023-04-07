public class EnemyCombatState : EnemyBaseState
{
    public EnemyCombatState(EnemóStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Move());
    }

    public override void UpdateState()
    {
    }
}

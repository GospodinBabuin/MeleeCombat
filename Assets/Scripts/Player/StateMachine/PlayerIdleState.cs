public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
    }

    public override void UpdateState()
    {

        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void CheckSwitchStates()
    {
        //if (Context.CombatMode)
        //{
          //  SwitchState(Factory.Combat());
        //}
    }
}

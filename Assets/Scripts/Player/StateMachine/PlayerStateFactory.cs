public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }

    public PlayerBaseState Combat()
    {
        return new PlayerCombatState(_context, this);
    }

    public PlayerMeleeAttackState MeleeAttack(bool dashToTarget, bool rollAttack)
    {
        return new PlayerMeleeAttackState(_context, this, dashToTarget, rollAttack);
    }

    public PlayerMoveState Move()
    {
        return new PlayerMoveState(_context, this);
    }
    
    public PlayerRollState Roll()
    {
        return new PlayerRollState(_context, this);
    }
}

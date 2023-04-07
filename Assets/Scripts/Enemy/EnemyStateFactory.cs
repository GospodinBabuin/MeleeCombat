using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory
{
    EnemóStateMachine _context;

    public EnemyStateFactory(EnemóStateMachine currentContext)
    {
        _context = currentContext;
    }

    public EnemyBaseState Idle()
    {
        return new EnemyIdleState(_context, this);
    }

    public EnemyBaseState Combat()
    {
        return new EnemyCombatState(_context, this);
    }

    public EnemyMeleeAttackState MeleeAttack()
    {
        return new EnemyMeleeAttackState(_context, this);
    }

    public EnemyMoveState Move()
    {
        return new EnemyMoveState(_context, this);
    }
}

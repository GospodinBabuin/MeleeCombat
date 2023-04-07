using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombatState : PlayerBaseState
{
    public PlayerCombatState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        AttackCheck();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Move());
    }

    public void AttackCheck()
    {
        if (Context.IsAttacking || !Context.InputActions.MeleeAttack) return;
        if (!Context.CanAttack)
        {
            Context.InputActions.MeleeAttack = false;
            return;
        }

        if (Context.EnemyDetection.CurrentTarget() == null)
        {
            if (Context.IsRolling)
            {
                SetSubStateAndEnter(Factory.MeleeAttack(false, true));
                return;
            }
            else
            {
                SetSubStateAndEnter(Factory.MeleeAttack(false, false));
                return;
            }
        }

        if (Context.IsRolling)
        {
            SetSubStateAndEnter(Factory.MeleeAttack(Context.InputActions.Run, true));
        }
        else
        {
            SetSubStateAndEnter(Factory.MeleeAttack(Context.InputActions.Run, false));
        }
    }

}

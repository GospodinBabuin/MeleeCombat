public class EnemyMeleeAttackState : EnemyBaseState
{
    public EnemyMeleeAttackState(EnemóStateMachine currentContext, EnemyStateFactory enemyStateFactory) : base(currentContext, enemyStateFactory)
    {
        IsRootState = false;
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsAttacking)
        {
            SwitchState(Factory.Move());
        }
    }

    public override void EnterState()
    {
        Context.Animator.SetTrigger(Context.AnimIDMeleeAttack);
        EquipMeleeWeapon(true);
        Context.IsAttacking = true;
        Context.CanAttack = false;
    }

    public override void ExitState()
    {
        EquipMeleeWeapon(false);
        Context.StartCoroutine(Context.AttackDelay());
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    private void EquipMeleeWeapon(bool equip)
    {
        Context.Weapon.gameObject.SetActive(equip);
    }
}

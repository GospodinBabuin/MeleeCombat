using System.Collections;
using UnityEngine;

public class PlayerMeleeAttackState : PlayerBaseState
{
    private bool _dashToTarget;
    private bool _rollAttack;
    public PlayerMeleeAttackState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory, bool dashToTarget, bool rollAttack) : base(currentContext, playerStateFactory)
    {
        InitializeSubState();
        _dashToTarget = dashToTarget;
        _rollAttack = rollAttack;
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
        EquipMeleeWeapon(true);
        Attack(_dashToTarget, _rollAttack);
    }

    public override void ExitState()
    {
        EquipMeleeWeapon(false);
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    private void Attack(bool needToGetCloser, bool rollAttack)
    {
        Context.IsAttacking = true;
        Context.InputActions.MeleeAttack = false;

        if (needToGetCloser)
        {
             Context.StartCoroutine(DashToTarget());
        }

        if (rollAttack)
        {
            Context.Animator.SetBool(Context.AnimIDRollAttack, true);
        }
        else
        {
            Context.Animator.SetTrigger(Context.AnimIDMeleeAttack);
        }


        IEnumerator DashToTarget()
        {
            EnemóStateMachine target = Context.EnemyDetection.CurrentTarget();

            while (Vector3.Distance(Context.transform.position, target.transform.position) > 2.0f)
            {
                Context.transform.LookAt(target.transform);
                Context.transform.rotation = Quaternion.Euler(0.0f, Context.transform.eulerAngles.y, 0.0f);
                Context.CharacterController.Move(Context.transform.forward * (Context.DashSpeed * Time.deltaTime) +
                    new Vector3(0.0f, Context.VerticalVelocity, 0.0f) * Time.deltaTime);

                yield return null;
            }
        }
    }

    private void EquipMeleeWeapon(bool equip)
    {
        Context.MeleeWeapon.gameObject.SetActive(equip);
    }
}

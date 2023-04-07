using System.Collections;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    private float _rollSpeed = 10.0f;

    public PlayerRollState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (!Context.IsRolling)
        {
            SwitchState(Factory.Move());
        }
    }

    public override void EnterState()
    {
        Context.StartCoroutine(Roll());
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    private IEnumerator Roll()
    {
        Context.IsRolling = true;
        Context.CanRoll = false;
        Context.Animator.SetTrigger(Context.AnimIDRoll);

        while (Context.IsRolling)
        {
            Context.CharacterController.Move(Context.transform.forward * (_rollSpeed * Time.deltaTime) +
                         new Vector3(0.0f, Context.VerticalVelocity, 0.0f) * Time.deltaTime);

            yield return null;
        }

        Context.StartCoroutine(RollDelay());
    }

    private IEnumerator RollDelay()
    {
        yield return new WaitForSeconds(Context.RollDelay);
        Context.CanRoll = true;
    }
}

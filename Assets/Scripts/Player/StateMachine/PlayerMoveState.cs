using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    private float _walkSpeed = 1.3f;
    private float _runSpeed = 4.0f;
    private float _speedChangeRate = 7.5f;
    private float _rotationSmoothTime = 0.10f;

    private float _speed;
    private float _rotationVelocity;

    public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        InitializeSubState();
    }

    public override void CheckSwitchStates()
    {
        if (Context.InputActions.Roll && !Context.IsRolling && Context.CanRoll)
        {
            SwitchState(Factory.Roll());
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
        Locomotion();

        CheckSwitchStates();
    }

    private void Locomotion()
    {
        float targetSpeed = Context.InputActions.Run ? _runSpeed : _walkSpeed;

        if (Context.InputActions.Move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(Context.CharacterController.velocity.x, 0.0f, Context.CharacterController.velocity.z).magnitude;
        if (currentHorizontalSpeed > _runSpeed) currentHorizontalSpeed = _runSpeed;
        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed,
                Time.deltaTime * _speedChangeRate);
        }
        else
        {
            _speed = targetSpeed;
        }

        if (_speed < 0.01f) _speed = 0f;

        if (Context.InputActions.Move != Vector2.zero)
        {
            Vector3 inputDirection = new Vector3(Context.InputActions.Move.x, 0.0f, Context.InputActions.Move.y).normalized;

            Context.TargetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              Context.MainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(Context.transform.eulerAngles.y, Context.TargetRotation,
            ref _rotationVelocity, _rotationSmoothTime);

            Context.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, Context.TargetRotation, 0.0f) * Vector3.forward;
            
        Context.CharacterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0.0f, Context.VerticalVelocity, 0.0f) * Time.deltaTime);

        Context.Animator.SetFloat(Context.AnimIDSpeed, _speed);
    }
}

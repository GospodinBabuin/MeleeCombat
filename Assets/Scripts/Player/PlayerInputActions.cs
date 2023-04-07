using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActions : MonoBehaviour
{
    public Vector2 Move;
    public Vector2 Look;
    public bool Run;
    public bool Roll;
    public bool MeleeAttack;
    private void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    private void OnLook(InputValue value)
    {
        LookInput(value.Get<Vector2>());
    }


    private void OnRun(InputValue value)
    {
        RunInput(value.isPressed);
    }

    private void OnMeleeAttack(InputValue value)
    {
        MeleeAttackInput(value.isPressed);
    }
    
    private void OnRoll(InputValue value)
    {
        RollInput(value.isPressed);
    }

    private void MoveInput(Vector2 newMoveDirection)
    {
        Move = newMoveDirection;
    }

    private void LookInput(Vector2 newLookDirection)
    {
        Look = newLookDirection;
    }

    private void RunInput(bool newRunState)
    {
        Run = newRunState;
    }

    private void MeleeAttackInput(bool newMeleeAttackState)
    {
        MeleeAttack = newMeleeAttackState;
    }
    
    private void RollInput(bool newRollState)
    {
        Roll = newRollState;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
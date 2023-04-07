using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private bool _isAttacking = false;
    private bool _canAttack = true;
    private float _lastAttackDelay = 1f;
    private Animator _animator;
    private EnemyDetection _enemyDetection;
    private PlayerInputActions _inputActions;
    private MeleeWeapon _meleeWeapon;

    private int _animIDMeleeAttack;
    private int _animIDSpellCast;
    private int _animIDBlock;


    public bool IsAttacking { get { return _isAttacking; } }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyDetection = GetComponentInChildren<EnemyDetection>();
        _inputActions = GetComponent<PlayerInputActions>();
        _meleeWeapon  = GetComponentInChildren<MeleeWeapon>();
        AssignAnimationIDs();
    }

    public void AttackCheck()
    {
        if (_isAttacking || !_inputActions.MeleeAttack) return;
        if (!_canAttack)
        {
            _inputActions.MeleeAttack = false;
            return;
        }

        if (_enemyDetection.CurrentTarget() == null)
        {
            Attack(false);
            return;
        }

        Attack(_inputActions.Run);
    }

    private void Attack(bool needToGetCloser)
    {
        _isAttacking = true;

        if (needToGetCloser)
        {
           // StartCoroutine(_playerLocomotion.DashToTarget());
        }

        _animator.SetTrigger(_animIDMeleeAttack);
        _inputActions.MeleeAttack = false;
    }

    public void EquipMeleeWeapon(bool equip)
    {
        _meleeWeapon.gameObject.SetActive(equip);
    }

    private IEnumerator LastAttackDelay()
    {
        yield return new WaitForSeconds(_lastAttackDelay);
        _canAttack = true;
    }

    private void AssignAnimationIDs()
    {
        _animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
        _animIDBlock = Animator.StringToHash("Block");
        _animIDSpellCast = Animator.StringToHash("SpellCast");
    }

    private void OnAttackEnd()
    {
        _isAttacking = false;
    }

    private void OnComboEnd()
    {
        _canAttack = false;
        StartCoroutine(LastAttackDelay());
    }
}

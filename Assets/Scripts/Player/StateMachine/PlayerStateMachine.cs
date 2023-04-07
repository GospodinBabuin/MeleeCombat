using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputActions))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerStateMachine : MonoBehaviour
{
    private Animator _animator;
    private EnemyDetection _enemyDetection;
    private PlayerInputActions _inputActions;
    private MeleeWeapon _meleeWeapon;
    private CharacterController _characterController;
    private Camera _mainCamera;

    [SerializeField] private AudioClip[] _footstepAudioClips;
    [SerializeField] private float _footstepAudioVolume = 0.3f;

    private float _targetRotation = 0.0f;
    private readonly float _verticalVelocity = -5.0f;

    private float _rollSpeed = 10.0f;
    [SerializeField] private bool _isRolling = false;
    [SerializeField] private bool _canRoll = true;
    private float _rollDelay = 0.5f;

    private float _dashSpeed = 15.0f;

    private int _animIDSpeed;
    private int _animIDRoll; 
    private int _animIDMeleeAttack;
    private int _animIDRollAttack;
    private int _animIDSpellCast;
    private int _animIDBlock;
    private int _animIDTakeHit;


    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool canAttack = true;
    [SerializeField] public float _lastAttackDelay = .5f;

    #region Public properties

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public MeleeWeapon MeleeWeapon { get => _meleeWeapon; set => _meleeWeapon = value; }
    public PlayerInputActions InputActions { get => _inputActions; set => _inputActions = value; }
    public EnemyDetection EnemyDetection { get => _enemyDetection; set => _enemyDetection = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public int AnimIDRoll { get => _animIDRoll; set => _animIDRoll = value; }
    public int AnimIDSpeed { get => _animIDSpeed; set => _animIDSpeed = value; }
    public float DashSpeed { get => _dashSpeed; set => _dashSpeed = value; }
    public bool IsRolling { get => _isRolling; set => _isRolling = value; }
    public float RollSpeed { get => _rollSpeed; set => _rollSpeed = value; }
    public float VerticalVelocity => _verticalVelocity;
    public CharacterController CharacterController { get => _characterController; set => _characterController = value; }
    public Camera MainCamera { get => _mainCamera; set => _mainCamera = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool CanAttack { get => canAttack; set => canAttack = value; }
    public int AnimIDMeleeAttack { get => _animIDMeleeAttack; set => _animIDMeleeAttack = value; }
    public int AnimIDSpellCast { get => _animIDSpellCast; set => _animIDSpellCast = value; }
    public int AnimIDBlock { get => _animIDBlock; set => _animIDBlock = value; }
    public int AnimIDRollAttack { get => _animIDRollAttack; set => _animIDRollAttack = value; }
    public int AnimIDTakeHit { get => _animIDTakeHit; set => _animIDTakeHit = value; }
    public float TargetRotation { get => _targetRotation; set => _targetRotation = value; }
    public float RollDelay { get => _rollDelay; set => _rollDelay = value; }
    public bool CanRoll { get => _canRoll; set => _canRoll = value; }

    #endregion

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _enemyDetection = GetComponentInChildren<EnemyDetection>();
        _inputActions = GetComponent<PlayerInputActions>();
        _meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        _characterController = GetComponent<CharacterController>();
        _mainCamera = Camera.main;

        _meleeWeapon.SetOwner(gameObject);
        AssignAnimationIDs();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Combat();
        _currentState.EnterState();
    }

    private void Update()
    {
        _currentState.UpdateStates();
        Debug.Log(_currentState);
        Debug.Log(_currentState._currentSubState);
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDRoll = Animator.StringToHash("Roll");
        _animIDMeleeAttack = Animator.StringToHash("MeleeAttack");
        _animIDRollAttack = Animator.StringToHash("RollAttack");
        _animIDTakeHit = Animator.StringToHash("TakeHit");
    }

    private void OnRollEnd()
    {
        _isRolling = false;
        _inputActions.Roll = false;
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (_footstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, _footstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(_footstepAudioClips[index], transform.TransformPoint(_characterController.center), _footstepAudioVolume);
            }
        }
    }

    private void OnAttackStart()
    {
        _meleeWeapon.BeginAttack();
    }

    private void OnAttackEnd()
    {
        IsAttacking = false;
        _meleeWeapon.EndAttack();
        _animator.SetBool(_animIDRollAttack, false);
    }

    private void OnComboEnd()
    {
        CanAttack = false;
        StartCoroutine(LastAttackDelay());

        IEnumerator LastAttackDelay()
        {
            yield return new WaitForSeconds(_lastAttackDelay);
            CanAttack = true;
        }
    }

}

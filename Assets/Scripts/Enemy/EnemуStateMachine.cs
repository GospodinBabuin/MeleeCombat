using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemóStateMachine : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private Transform _playerTransform;
    private Animator _animator;
    private MeleeWeapon _weapon;

    private EnemyBaseState _currentState;
    private EnemyStateFactory _states;

    private int _animIDSpeed;
    private int _animIDTakeHit;
    private int _animIDMeleeAttack;

    [SerializeField] private float _stopDistance = 1.4f;
    [SerializeField] private bool _canAttack = true;
    [SerializeField] private float _attackDeleay = 0.5f;

    [SerializeField] private bool _isAttacking = false;

    public EnemyBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public EnemyStateFactory States { get => _states; set => _states = value; }
    public NavMeshAgent NavMeshAgent { get => _navMeshAgent; set => _navMeshAgent = value; }
    public Transform PlayerTransform { get => _playerTransform; set => _playerTransform = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public MeleeWeapon Weapon { get => _weapon; set => _weapon = value; }
    public int AnimIDSpeed { get => _animIDSpeed; set => _animIDSpeed = value; }
    public float StopDistance { get => _stopDistance; set => _stopDistance = value; }
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }
    public int AnimIDTakeHit { get => _animIDTakeHit; set => _animIDTakeHit = value; }
    public int AnimIDMeleeAttack { get => _animIDMeleeAttack; set => _animIDMeleeAttack = value; }
    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Animator = GetComponent<Animator>();
        Weapon = GetComponentInChildren<MeleeWeapon>();
        Weapon.SetOwner(gameObject);

        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDTakeHit = Animator.StringToHash("TakeHit");
        _animIDMeleeAttack = Animator.StringToHash("MeleeAttack");

        States = new EnemyStateFactory(this);
        CurrentState = States.Combat();
        CurrentState.EnterState();
    }

    private void FixedUpdate()
    {
        _currentState.UpdateStates();
        Debug.Log(_currentState);
        Debug.Log(_currentState._currentSubState);
    }

    public IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(_attackDeleay);
        CanAttack = true;
    }

    private void OnAttackEnd()
    {
        _isAttacking = false;
        _weapon.EndAttack();

    }

    private void OnAttackStart()
    {
        _weapon.BeginAttack();
    }
}
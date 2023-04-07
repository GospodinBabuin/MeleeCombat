using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private PlayerInputActions _inputActions;

    private Camera _camera;
    private Vector3 _forward;
    private Vector3 _right;

    private Vector3 _inputDirection;
    [SerializeField] private Enem�StateMachine _currentTarget;

    [SerializeField] private LayerMask _enemyLayer;

    private void Start()
    {
        _inputActions = GetComponentInParent<PlayerInputActions>();
        _camera = Camera.main;
    }

    private void Update()
    {
        _forward = _camera.transform.forward;
        _right = _camera.transform.right;

        _forward.y = 0;
        _right.y = 0;

        _forward.Normalize();
        _right.Normalize();

        _inputDirection = _forward * _inputActions.Move.y + _right * _inputActions.Move.x;
        _inputDirection = _inputDirection.normalized;

        if (Physics.SphereCast(transform.position, 3f, _inputDirection, out RaycastHit info, 2.5f, _enemyLayer))
        {
            if (info.collider.transform.GetComponent<Enem�StateMachine>()) //ISAttackable
            {
                _currentTarget = info.collider.transform.GetComponent<Enem�StateMachine>();
            }
        }
        else
        {
            _currentTarget = null;
        }
    }

    public Enem�StateMachine CurrentTarget()
    {
        return _currentTarget;
    }

    public void SetCurrentTarget(Enem�StateMachine target)
    {
        _currentTarget = target;
    }

    public float InputMagnitude()
    {
        return _inputDirection.magnitude;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, _inputDirection);
        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawWireSphere(transform.position, 2.5f);
        if (CurrentTarget() != null)
            Gizmos.DrawSphere(CurrentTarget().transform.position, .5f);
    }
}

public abstract class EnemyBaseState
{
    private bool _isRootState = false;
    private EnemóStateMachine _context;
    private EnemyStateFactory _factory;
    public EnemyBaseState _currentSubState;
    private EnemyBaseState _currentSuperState;

    public bool IsRootState { set { _isRootState = value; } }
    public EnemóStateMachine Context { get { return _context; } }
    public EnemyStateFactory Factory { get { return _factory; } }
    public EnemyBaseState CurrentSubState { get { return _currentSubState; } }

    public EnemyBaseState(EnemóStateMachine currentContext, EnemyStateFactory enemyStateFactory)
    {
        _context = currentContext;
        _factory = enemyStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
        {
            _context.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(EnemyBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(EnemyBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    protected void SetSubStateAndEnter(EnemyBaseState newSubState)
    {
        _currentSubState.ExitState();

        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

        _currentSubState.EnterState();
    }
}

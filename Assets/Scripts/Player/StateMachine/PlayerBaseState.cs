using System.Diagnostics;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _context;
    private PlayerStateFactory _factory;
    public PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    public bool IsRootState { set { _isRootState = value; } }
    public PlayerStateMachine Context { get { return _context; } }
    public PlayerStateFactory Factory { get { return _factory; } }
    public PlayerBaseState CurrentSubState { get { return _currentSubState; } }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _context = currentContext;
        _factory = playerStateFactory;
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

    protected void SwitchState(PlayerBaseState newState)
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

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    protected void SetSubStateAndEnter(PlayerBaseState newSubState)
    {
        _currentSubState.ExitState();

        _currentSubState = newSubState;
        newSubState.SetSuperState(this);

        _currentSubState.EnterState();
    }
}

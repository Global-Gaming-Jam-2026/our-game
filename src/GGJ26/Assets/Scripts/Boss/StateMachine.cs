using UnityEngine;

public class StateMachine : MonoBehaviour
{
    Entity _controller;
    StateBase _currentState;

    public StateBase State => _currentState;

    public void SetController(Entity controller)
    {
        _controller = controller;
        InitAllStates();
    }

    void InitAllStates()
    {
        StateBase[] allStates = GetComponentsInChildren<StateBase>();
        foreach (var state in allStates)
        {
            state.SetController(_controller);
        }
    }

    public void SetState(StateBase newState, bool forceReset = false)
    {
        if (_currentState == null || _currentState.IsDone || forceReset)
        {
            Debug.Log("Switching");
            _currentState?.OnStateExit();
            _currentState = newState;
            _currentState.OnStateEnter();
        }
    }

    private void Update()
    {
        _currentState.OnStateUpdate();
    }
}

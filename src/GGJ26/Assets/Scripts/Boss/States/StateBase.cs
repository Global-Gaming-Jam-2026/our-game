using UnityEngine;

public class StateBase : MonoBehaviour
{
    protected StateBase _subState;
    protected Entity Controller;
    protected bool _isDone;
    protected float _startTime;

    public float StateTime => Time.time - _startTime;

    public bool IsDone => _isDone;

    public void SetController(Entity controller)
    {
        Controller = controller;
    }

    // ############## OVERRIDABLE METHODS ##############
    public virtual void OnStateEnter()
    {
        _isDone = false;
        _startTime = Time.time;
    }

    public virtual void OnStateExit()
    {

    }

    public virtual void OnStateUpdate()
    {
        _subState?.OnStateUpdate();
    }

    public virtual void OnStateFixedUpdate()
    {
        _subState?.OnStateFixedUpdate();
    }
}

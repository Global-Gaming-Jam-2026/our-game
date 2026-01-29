using UnityEngine;

public class IdleState : StateBase
{
    [SerializeField]
    [Min(1.0f)] float _minIdleTime;

    [SerializeField]
    [Min(1.0f)] float _maxIdleTime;

    float _endTime;

    public override void OnStateEnter()
    {
        Debug.Log($"Start idle State");
        base.OnStateEnter();
        DetermineIdleTime();
    }

    void DetermineIdleTime()
    {
        float waitTime = Random.Range(_minIdleTime, _maxIdleTime);
        _endTime = Time.time + waitTime;
    }

    public override void OnStateUpdate()
    {
        if (Time.time < _endTime)
            return;
        _isDone = true;
    }
}

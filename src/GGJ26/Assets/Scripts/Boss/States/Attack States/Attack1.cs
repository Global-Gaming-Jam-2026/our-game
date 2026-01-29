using UnityEngine;

public class Attack1 : StateBase
{
    float _endTime;
    public override void OnStateEnter()
    {
        Debug.Log("Start Attack State");
        base.OnStateEnter();
        _endTime = Time.time + 3f;
    }

    public override void OnStateUpdate()
    {
        if (Time.time < _endTime)
            return;
        _isDone = true;
    }
}

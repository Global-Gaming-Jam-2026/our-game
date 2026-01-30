using System.Collections;
using UnityEngine;

public class BossController : Entity
{
    

    [Header("States")]
    [SerializeField] BossIdleState _idleState;
    [SerializeField] BossAttackState _attackState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitMachine();
        _machine.SetState(_idleState, true);
    }

    private void Update()
    {
        SelectState();
    }

    void SelectState()
    {
        if (_machine.State == _idleState)
        {
            _machine.SetState(_attackState);
        }
        else
        {
            _machine.SetState(_idleState);
        }
    }
}

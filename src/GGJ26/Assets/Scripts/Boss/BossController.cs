using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    StateMachine _machine;

    [SerializeField] Rigidbody2D _body;

    [Header("States")]
    [SerializeField] IdleState _idleState;
    [SerializeField] AttackState _attackState;

    public Rigidbody2D Body => _body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _machine = GetComponentInChildren<StateMachine>();
        _machine.SetController(this);
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

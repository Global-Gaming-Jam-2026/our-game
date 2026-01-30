using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _body;

    protected StateMachine _machine;
    protected Animator _animator;
    protected bool _isOperating;

    public Rigidbody2D Body => _body;
    public Animator Animator => _animator;

    protected void InitMachine()
    {
        _machine = GetComponentInChildren<StateMachine>();
        _machine.SetController(this);
    }

    protected void InitAnimator()
    {
        _animator = GetComponent<Animator>();
    }
}

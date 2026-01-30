using UnityEngine;

public class Entity : MonoBehaviour
{
    protected StateMachine _machine;

    [SerializeField] protected Rigidbody2D _body;

    public Rigidbody2D Body => _body;

    protected void InitMachine()
    {
        _machine = GetComponentInChildren<StateMachine>();
        _machine.SetController(this);
    }
}

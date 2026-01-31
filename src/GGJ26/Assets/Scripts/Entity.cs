using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _body;
    [SerializeField] protected SFXPlayer _sfxPlayer;

    protected StateMachine _machine;
    protected Animator _animator;
    protected bool _isOperating;

    public Rigidbody2D Body => _body;
    public Animator Animator => _animator;

    public SFXPlayer SFXPlayer => _sfxPlayer;

    protected void InitMachine()
    {
        _machine = GetComponentInChildren<StateMachine>();
        _machine.SetController(this);
    }

    protected void InitAnimator()
    {
        _animator = GetComponent<Animator>();
    }

    protected void InitSFXPlayer()
    {
        _sfxPlayer = GetComponentInChildren<SFXPlayer>();
    }

    protected void InitComponents()
    {
        InitMachine();
        InitAnimator();
        InitSFXPlayer();
    }
}

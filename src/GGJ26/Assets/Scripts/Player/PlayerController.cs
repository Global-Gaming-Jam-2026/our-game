using System;
using UnityEngine;

public class PlayerController : Entity
{
    //################### SINGLETON ###################
    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;

        _actions = new PlayerInputActions();
    }
    //################### END SINGLETON ###############

    //################### INPUT #######################
    PlayerInputActions _actions;

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }
    //################### END INPUT ##################

    [SerializeField] LayerMask _groundMask;
    [SerializeField] float _runSpeed;

    [Header("States")]
    [SerializeField] PlayerIdleState _idleState;
    [SerializeField] PlayerRunState _runState;
    [SerializeField] PlayerJumpState _jumpState;
    [SerializeField] PlayerFallState _fallState;
    [SerializeField] PlayerWhipState _whipState;

    Collider2D _groundCollider;

    public PlayerInputActions InputActions => _actions;
    public float RunSpeed => _runSpeed;

    private void Start()
    {
        InitAnimator();
        InitMachine();
        _machine.SetState(_idleState);
    }

    bool CastForGrounded()
    {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.8f, 0.025f), 0f, _groundMask);
        _groundCollider = overlaps.Length > 0 ? overlaps[0] : null;
        return overlaps.Length > 0;
    }

    bool IsMoving(out float xInput)
    {
        xInput = _actions.Player.Move.ReadValue<Vector2>().x;
        return Mathf.Abs(xInput) > Mathf.Epsilon;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.8f, 0.025f, 0.1f));
    }

    private void Update()
    {
        SelectState();
        if (_machine.State != _whipState)
        {
            HandleFacingDirection();
        }
    }

    private void HandleFacingDirection()
    {
        if (IsMoving(out float xInput))
        {
            int direction = Mathf.RoundToInt(Mathf.Sign(xInput));
            float yRotation = direction > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(Vector3.up * yRotation);
        }
    }

    void SelectState()
    {
        //if (_actions.Player.Attack.WasPressedThisFrame())
        //{
        //    _machine.SetState(_whipState);
        //}
        bool isGrounded = CastForGrounded();
        if (isGrounded)
        {
            if (IsMoving(out float xInput))
            {
                _machine.SetState(_runState);
            }
            else
            {
                _machine.SetState(_idleState);
            }
            if (_actions.Player.Jump.WasPressedThisFrame())
            {
                _machine.SetState(_jumpState);
            }
        }
        else if (_body.linearVelocity.y <= 0.01f)
        {
            _machine.SetState(_fallState);
        }
    }
}

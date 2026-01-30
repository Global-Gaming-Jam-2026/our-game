using UnityEngine;

public class PlayerJumpState : StateBase
{
    [SerializeField][Min(1.0f)] float _jumpForce;
    [SerializeField][Min(1.0f)] float _lowJumpMultiplier;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        Vector2 vel = Controller.Body.linearVelocity;
        vel.y = _jumpForce;
        Controller.Body.linearVelocity = vel;
    }

    public override void OnStateFixedUpdate()
    {
        PlayerController player = Controller as PlayerController;
        Vector2 vel = Controller.Body.linearVelocity;
        if (player != null)
        {
            float xInput = player.InputActions.Player.Move.ReadValue<Vector2>().x;
            vel.x = Mathf.Lerp(vel.x, xInput * player.RunSpeed, 10 * Time.fixedDeltaTime);
            if (!player.InputActions.Player.Jump.IsPressed())
            {
                vel.y += Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
        Controller.Body.linearVelocity = vel;
        _isDone = Controller.Body.linearVelocity.y <= 0;
    }

    public override void OnStateExit()
    {
        _isDone = true;
    }
}

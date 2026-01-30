using UnityEngine;

public class PlayerFallState : StateBase
{
    [SerializeField][Min(1.0f)] float _fallMultiplier;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _isDone = true;
    }

    public override void OnStateFixedUpdate()
    {
        Vector2 vel = Controller.Body.linearVelocity;
        vel.y += Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime;

        PlayerController player = Controller as PlayerController;
        if (player != null)
        {
            float xInput = player.InputActions.Player.Move.ReadValue<Vector2>().x;
            vel.x = Mathf.Lerp(vel.x, xInput * player.RunSpeed, 10 * Time.fixedDeltaTime);
        }

        Controller.Body.linearVelocity = vel;
    }
}

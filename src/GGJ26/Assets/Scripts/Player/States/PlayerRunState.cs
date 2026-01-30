using UnityEngine;

public class PlayerRunState : StateBase
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _isDone = true;
    }

    public override void OnStateUpdate()
    {
        PlayerController player = Controller as PlayerController;
        if (player != null)
        {
            float xInput = player.InputActions.Player.Move.ReadValue<Vector2>().x;
            Vector2 desiredVel = xInput * player.RunSpeed * Vector2.right;
            player.Body.linearVelocity = Vector2.Lerp(player.Body.linearVelocity, desiredVel, 10 * Time.deltaTime);
        }
    }
}

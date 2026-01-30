using UnityEngine;

public class PlayerIdleState : StateBase
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _isDone = true;
    }

    public override void OnStateFixedUpdate()
    {
        Controller.Body.linearVelocity = Vector2.Lerp(Controller.Body.linearVelocity, Vector2.zero, 10 * Time.fixedDeltaTime);
    }
}

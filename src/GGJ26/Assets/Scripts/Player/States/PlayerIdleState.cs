using UnityEngine;

public class PlayerIdleState : StateBase
{
    [SerializeField] AnimationClip _idleAnimation;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Controller.Animator.Play(_idleAnimation.name);
        _isDone = true;
    }

    public override void OnStateFixedUpdate()
    {
        Controller.Body.linearVelocity = Vector2.Lerp(Controller.Body.linearVelocity, Vector2.zero, 10 * Time.fixedDeltaTime);
    }
}

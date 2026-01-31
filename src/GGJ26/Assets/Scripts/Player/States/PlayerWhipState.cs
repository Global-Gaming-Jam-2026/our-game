using UnityEngine;

public class PlayerWhipState : StateBase
{
    [SerializeField] AnimationClip _sideWhipAnimation;
    [SerializeField] AnimationClip _upWhipAnimation;
    [SerializeField] AudioClip _whipSFX;
    [SerializeField] float _fallMultiplier;

    AnimationClip _clipToPlay;

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        _clipToPlay = DetermineDirection();
        Controller.Animator.Play(_clipToPlay.name);
        Controller.SFXPlayer.PlaySFX(_whipSFX);
    }

    AnimationClip DetermineDirection()
    {
        Vector2 mousePosScreen = PlayerController.Instance.InputActions.Player.MousePosition.ReadValue<Vector2>();
        Vector2 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScreen);
        Vector2 directionToMouse = (mousePosWorld - (Vector2)Controller.Body.transform.position).normalized;

        if (Mathf.Abs(Vector2.Angle(Vector2.up, directionToMouse)) < 30)
        {
            return _upWhipAnimation;
        }

        float rotationAngle = Mathf.Sign(directionToMouse.x) > 0 ? 0f : 180f;
        Controller.transform.rotation = Quaternion.Euler(rotationAngle * Vector3.up);
        return _sideWhipAnimation;
    }

    public override void OnStateFixedUpdate()
    {
        PlayerController player = Controller as PlayerController;
        if (player != null)
        {
            float xInput = player.InputActions.Player.Move.ReadValue<Vector2>().x;
            float desiredX = xInput * player.RunSpeed;
            Vector2 vel = player.Body.linearVelocity;
            vel.x = Mathf.Lerp(vel.x, desiredX, 10 * Time.deltaTime);

            if (!player.IsGrounded)
            {
                vel.y += Physics2D.gravity.y * (_fallMultiplier - 1) * Time.fixedDeltaTime; 
            }

            player.Body.linearVelocity = vel;
        }
    }

    public override void OnStateUpdate()
    {
        _isDone = StateTime > _clipToPlay?.length * 0.8f;
    }
}

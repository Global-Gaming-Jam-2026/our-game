using UnityEngine;

public class PlayerWhipState : StateBase
{
    [SerializeField] AnimationClip _sideWhipAnimation;
    [SerializeField] AnimationClip _upWhipAnimation;
    [SerializeField] AudioClip _whipSFX;

    AnimationClip _clipToPlay;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        float upInput = PlayerController.Instance.InputActions.Player.Move.ReadValue<Vector2>().y;
        _clipToPlay = upInput > 0 ? _upWhipAnimation : _sideWhipAnimation;
        Debug.Log(_clipToPlay.name);
        Controller.Animator.Play(_clipToPlay.name);
        Controller.SFXPlayer.PlaySFX(_whipSFX);
    }

    public override void OnStateFixedUpdate()
    {
        PlayerController player = Controller as PlayerController;
        if (player != null)
        {
            float xInput = player.InputActions.Player.Move.ReadValue<Vector2>().x;
            Vector2 desiredVel = xInput * player.RunSpeed * Vector2.right;
            player.Body.linearVelocity = Vector2.Lerp(player.Body.linearVelocity, desiredVel, 10 * Time.deltaTime);
        }
    }

    public override void OnStateUpdate()
    {
        _isDone = StateTime > _clipToPlay?.length * 0.8f;
    }
}

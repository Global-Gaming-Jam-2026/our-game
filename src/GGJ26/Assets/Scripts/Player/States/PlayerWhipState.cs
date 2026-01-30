using UnityEngine;

public class PlayerWhipState : StateBase
{
    [SerializeField] AnimationClip _sideWhipAnimation;
    [SerializeField] AnimationClip _upWhipAnimation;

    AnimationClip _clipToPlay;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        float upInput = PlayerController.Instance.InputActions.Player.Move.ReadValue<Vector2>().y;
        _clipToPlay = upInput > 0 ? _upWhipAnimation : _sideWhipAnimation;
    }

    public override void OnStateUpdate()
    {
        _isDone = StateTime > _clipToPlay?.length * 0.8f;
    }
}

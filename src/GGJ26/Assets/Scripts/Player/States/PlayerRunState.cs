using UnityEngine;

public class PlayerRunState : StateBase
{
    [SerializeField] AudioClip[] _stepSFXLibrary;
    [SerializeField] float _stepSFXInterval;

    float _stepIntervalCountdown;
    int _currentStepIndex;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Controller.Animator.SetBool("IsRunning", true);
        _stepIntervalCountdown = 0;
        _isDone = true;
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
        PlaySteppingSFX();
    }

    void PlaySteppingSFX()
    {
        if (_stepIntervalCountdown > 0)
        {
            _stepIntervalCountdown -= Time.deltaTime;
            return;
        }

        _stepIntervalCountdown = _stepSFXInterval;
        _currentStepIndex = (_currentStepIndex + 1) % _stepSFXLibrary.Length;
        Controller.SFXPlayer.PlaySFX(_stepSFXLibrary[_currentStepIndex]);
    }
}

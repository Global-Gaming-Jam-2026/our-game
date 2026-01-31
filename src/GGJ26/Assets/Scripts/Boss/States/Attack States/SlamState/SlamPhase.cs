using DG.Tweening;
using UnityEngine;

public class SlamPhase : StateBase
{
    [Header("Slam Settings")]
    [SerializeField] private float slamSpeed = 0.35f;
    [SerializeField] private float returnSpeed = 0.8f;
    [SerializeField] AudioClip _impactSFX;

    [Header("Damage")]
    [SerializeField] private Collider2D _playerDamageCollider;
    [SerializeField] private float damageActiveDuration = 0.3f;

    private Sequence _slamSequence;

    public Vector3 OriginalPosition { get; set; }

    public override void OnStateEnter()
    {
        base.OnStateEnter();

        AnimateSlam();
    }

    private void AnimateSlam()
    {
        //we want to reach the ground (y position of -4) so we take the direction to the player and extrapolate to find the target position
        Vector3 playerDirection = (PlayerController.Instance.transform.position - Controller.Body.transform.position).normalized;
        float _neededDirectionMultiplier = (-4f - Controller.Body.transform.position.y) / playerDirection.y;
        Vector3 targetPosition = Controller.Body.transform.position + _neededDirectionMultiplier * playerDirection;
        
        _slamSequence = DOTween.Sequence();


        // Phase 3: Slam to player position
        _slamSequence
            .Append(Controller.Body.transform.DOMove(targetPosition, slamSpeed * CooldownHolder.GlobalCooldownMultiplier)
                .SetEase(Ease.InQuint))
            .AppendCallback(OnSlamImpact);

        // Phase 4: Impact shake
        _slamSequence
            .Append(Controller.Body.transform.DOShakePosition(0.3f, 0.3f, 30));

        // Phase 5: Return to original position
        _slamSequence
            .Append(Controller.Body.transform.DOMove(OriginalPosition, returnSpeed)
                .SetEase(Ease.OutCubic))
            .OnComplete(() => _isDone = true);

        _playerDamageCollider.gameObject.SetActive(true);
        _slamSequence.Play();
    }

    private void OnSlamImpact()
    {
        Controller.SFXPlayer.PlaySFX(_impactSFX);
        // Activate damage collider
        if (_playerDamageCollider != null)
        {
            DOVirtual.DelayedCall(damageActiveDuration, () =>
                _playerDamageCollider.gameObject.SetActive(false));
        }
    }
}

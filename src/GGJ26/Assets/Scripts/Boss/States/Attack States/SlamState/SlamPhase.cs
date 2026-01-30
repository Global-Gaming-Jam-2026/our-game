using DG.Tweening;
using TMPro;
using UnityEngine;

public class SlamPhase : StateBase
{
    [Header("Slam Settings")]
    [SerializeField] private float slamSpeed = 0.35f;
    [SerializeField] private float returnSpeed = 0.8f;

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
        Vector3 targetPosition = PlayerController.Instance.transform.position;
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
    }

    private void OnSlamImpact()
    {
        // Activate damage collider
        if (_playerDamageCollider != null)
        {
            _playerDamageCollider.gameObject.SetActive(true);
            DOVirtual.DelayedCall(damageActiveDuration, () =>
                _playerDamageCollider.gameObject.SetActive(false));
        }
    }
}

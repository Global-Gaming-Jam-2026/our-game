using UnityEngine;
using DG.Tweening;

public class SlamAttack : StateBase
{
    [Header("Slam Settings")]
    [SerializeField] private float riseHeight = 3f;
    [SerializeField] private float telegraphDuration = 1.2f;
    [SerializeField] private float slamSpeed = 0.35f;
    [SerializeField] private float returnSpeed = 0.8f;

    [Header("Damage")]
    [SerializeField] private Collider2D _playerDamageCollider;
    [SerializeField] private float damageActiveDuration = 0.3f;

    [Header("Visual")]
    [SerializeField] private GameObject _shadowIndicator;

    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    private Sequence _slamSequence;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Starting slam attack!");

        _originalPosition = Controller.Body.transform.position;

        // Target player position (both X and Y)
        _targetPosition = PlayerController.Instance.transform.position;

        SetupShadow();
        AnimateSlam();
    }

    private void SetupShadow()
    {
        if (_shadowIndicator != null)
        {
            _shadowIndicator.transform.position = _targetPosition;
            _shadowIndicator.transform.localScale = Vector3.zero;
            _shadowIndicator.SetActive(true);
        }
    }

    private void AnimateSlam()
    {
        _slamSequence = DOTween.Sequence();

        // Phase 1: Rise up + shadow grows (telegraph)
        _slamSequence
            .Append(Controller.Body.transform.DOMoveY(_originalPosition.y + riseHeight, telegraphDuration * 0.6f)
                .SetEase(Ease.OutCubic))
            .Join(_shadowIndicator != null
                ? _shadowIndicator.transform.DOScale(Vector3.one, telegraphDuration * 0.6f).SetEase(Ease.OutCubic)
                : DOTween.Sequence());

        // Phase 2: Hold at apex
        _slamSequence.AppendInterval(telegraphDuration * 0.4f);

        // Phase 3: Slam to player position
        _slamSequence
            .Append(Controller.Body.transform.DOMove(_targetPosition, slamSpeed)
                .SetEase(Ease.InQuint))
            .AppendCallback(OnSlamImpact);

        // Phase 4: Impact shake
        _slamSequence
            .Append(Controller.Body.transform.DOShakePosition(0.3f, 0.3f, 30));

        // Phase 5: Return to original position
        _slamSequence
            .Append(Controller.Body.transform.DOMove(_originalPosition, returnSpeed)
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

        // Hide shadow
        if (_shadowIndicator != null)
        {
            _shadowIndicator.SetActive(false);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        _slamSequence?.Kill();

        if (_shadowIndicator != null)
            _shadowIndicator.SetActive(false);

        if (_playerDamageCollider != null)
            _playerDamageCollider.gameObject.SetActive(false);
    }
}

using DG.Tweening;
using UnityEngine;

public class TelegraphPhase : StateBase
{
    [SerializeField] private float riseHeight = 3f;
    [SerializeField] private float telegraphDuration = 1.2f;
    [SerializeField] AudioClip[] _bearRoarLibrary;

    [Header("Visual")]
    [SerializeField] private GameObject _shadowIndicator;

    private Vector3 _originalPosition;
    private Vector3 _targetPosition;
    private Sequence _telegraphSequence;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _originalPosition = Controller.Body.transform.position;
        Controller.SFXPlayer.PlaySFX(_bearRoarLibrary[Random.Range(0, _bearRoarLibrary.Length)]);
        SetupShadow();
        Animate();
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

    void Animate()
    {
        // Target player position (both X and Y)
        Vector3 targetPosition = PlayerController.Instance.transform.position;

        _telegraphSequence = DOTween.Sequence();

        // Phase 1: Rise up + shadow grows (telegraph)
        _telegraphSequence
            .Append(Controller.Body.transform.DOMoveY(_originalPosition.y + riseHeight, telegraphDuration * 0.6f)
                .SetEase(Ease.OutCubic))
            .Join(_shadowIndicator != null
                ? _shadowIndicator.transform.DOScale(Vector3.one * 0.075f, telegraphDuration * 0.6f).SetEase(Ease.OutCubic)
                : DOTween.Sequence())
            .OnUpdate(() =>
            {
                if (_shadowIndicator != null)
                {
                    targetPosition = PlayerController.Instance.transform.position;
                    _shadowIndicator.transform.position = targetPosition;
                }
            });

        // Phase 2: Hold at apex
        _telegraphSequence.AppendInterval(telegraphDuration * 0.4f);

        _telegraphSequence.OnComplete(() =>
        {
            _isDone = true;
            // Hide shadow
            if (_shadowIndicator != null)
            {
                _shadowIndicator.SetActive(false);
            }
        });

        _telegraphSequence.Play();
    }
}

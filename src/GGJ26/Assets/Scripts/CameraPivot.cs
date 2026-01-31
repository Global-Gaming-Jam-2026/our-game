using DG.Tweening;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    [SerializeField] SpriteRenderer _faderRenderer;

    Sequence _rotateSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnCameraFlipStart += FlipCamera;
    }

    void FlipCamera()
    {
        _rotateSequence = DOTween.Sequence();

        _rotateSequence.Append(transform.DOShakePosition(0.75f, Vector2.right))
            .Append(_faderRenderer.DOFade(1f, 0.75f))
            .Append(transform.DORotate(180 * Vector3.up, 1.5f).SetEase(Ease.InOutSine))
            .Append(_faderRenderer.DOFade(0f, 0.75f))
            .OnComplete(() => EventBus.Instance.OnCameraFlipEnd?.Invoke());
    }
}

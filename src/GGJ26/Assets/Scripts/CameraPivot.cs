using DG.Tweening;
using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    Sequence _rotateSequence;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EventBus.Instance.OnCameraFlipStart += FlipCamera;
    }

    void FlipCamera()
    {
        _rotateSequence = DOTween.Sequence();

        _rotateSequence.Append(transform.DOShakePosition(1f, Vector2.right))
            .Append(transform.DORotate(180 * Vector3.up, 2f).SetEase(Ease.InOutSine))
            .OnComplete(() => EventBus.Instance.OnCameraFlipEnd?.Invoke());
    }
}

using DG.Tweening;
using UnityEngine;

public class SpritaAlphaModulator : MonoBehaviour
{
    SpriteRenderer _renderer;
    Sequence _animateSequence;

    private void OnEnable()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animateSequence = DOTween.Sequence();
        _animateSequence.Append(_renderer.DOFade(1f, 0.3f))
            .Append(_renderer.DOFade(0.6f, 0.25f).SetLoops(30, LoopType.Yoyo));
        _animateSequence.Play();
    }

    private void OnDisable()
    {
        _animateSequence.Kill();
    }
}

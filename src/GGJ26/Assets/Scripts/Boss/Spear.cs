using DG.Tweening;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Transform _originalParent;
    Rigidbody2D _body;
    Vector3 _speed;

    Sequence _launchSequence;

    private void OnEnable()
    {
        _originalParent = transform.parent;
        float zAngle = transform.eulerAngles.z;
        transform.DORotate((zAngle + 360) * Vector3.forward, 0.5f, RotateMode.FastBeyond360);
    }

    public void LaunchDelayed(int index)
    {
        _launchSequence = DOTween.Sequence();
        _launchSequence
            .Append(transform.DOLocalMove(transform.localPosition + 0.5f * transform.right, 0.3f).SetDelay(1.2f + 0.35f * index)
                .OnComplete(() => LaunchSpear()));
        _launchSequence.Play();
    }

    void LaunchSpear()
    {
        _body = GetComponent<Rigidbody2D>();
        transform.SetParent(null);
        DOTween.To(() => _speed, x => _speed = x, -transform.right * 12.5f, 0.5f).OnComplete(() => _body.linearVelocity = _speed);
    }

    private void OnBecameInvisible()
    {
        _launchSequence.Kill();
        transform.parent = _originalParent;
        gameObject.SetActive(false);
    }
}

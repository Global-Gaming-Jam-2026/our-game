using DG.Tweening;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Transform _originalParent;
    Rigidbody2D _body;
    Vector3 _speed;

    private void OnEnable()
    {
        float zAngle = transform.eulerAngles.z;
        transform.DORotate((zAngle + 360) * Vector3.forward, 0.5f, RotateMode.FastBeyond360);
    }

    public void LaunchDelayed(int index)
    {
        _originalParent = transform.parent;
        _body = GetComponent<Rigidbody2D>();
        transform.DOLocalMove(transform.localPosition + 0.5f * transform.right, 0.3f).SetDelay(1.2f + 0.35f * index)
            .OnComplete(() => LaunchSpear());
    }

    void LaunchSpear()
    {
        transform.SetParent(null);
        DOTween.To(() => _speed, x => _speed = x, -transform.right * 12.5f, 0.5f).OnComplete(() => _body.linearVelocity = _speed);
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
        transform.parent = _originalParent;
    }
}

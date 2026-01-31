using DG.Tweening;
using UnityEngine;

public class IntroRotator : MonoBehaviour
{
    [SerializeField] BossController _controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sequence appearSequence = DOTween.Sequence();

        appearSequence.Append(transform.DOMove(new Vector3(0, 2, 0), 3f))
            .Join(transform.DORotate(Vector3.up * 1080, 3f, RotateMode.WorldAxisAdd))
            .OnComplete(() =>
            {
                _controller.gameObject.SetActive(true);
                gameObject.SetActive(false);
            });
    }
}

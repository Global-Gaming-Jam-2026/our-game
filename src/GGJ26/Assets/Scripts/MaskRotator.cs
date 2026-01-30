using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MaskRotator : MonoBehaviour
{
    [SerializeField] List<GameObject> _masks;
    [SerializeField] Collider2D _bossCollider;

    bool _inArrangement;

    public bool InArrangement => _inArrangement;

    void RearrangeMasks()
    {
        _inArrangement = true;
        float angleInterval = 360f / _masks.Count;
        for (int i = 0; i < _masks.Count; i++)
        {
            Vector3 desiredPos = Quaternion.Euler(i * angleInterval * Vector3.up) * Vector3.forward * 2f;
            Quaternion desiredRot = Quaternion.Euler(i * angleInterval * Vector3.up);

            Sequence orderSequence = DOTween.Sequence();
            orderSequence.Append(_masks[i].transform.DOLocalMove(desiredPos, 0.75f))
                .Join(_masks[i].transform.DOLocalRotate(i * angleInterval * Vector3.up, 0.75f)).
                OnComplete(() => _masks[i].transform.LookAt(transform));

            if (i == _masks.Count - 1)
            {
                orderSequence.OnComplete(() =>
                {
                    _inArrangement = false;
                    _bossCollider.enabled = true;
                });
            }

            orderSequence.Play();
        }
    }

    public void AddMask(GameObject mask)
    {
        if (!_masks.Contains(mask))
        {
            _masks.Add(mask);
            RearrangeMasks();
        }
    }

    int[][] _rotations;

    private void Start()
    {
        _rotations = new int[4][];
        _rotations[0] = new int[] { 0, 180 };
        _rotations[1] = new int[] { 180, 60, 300 };
        _rotations[2] = new int[] { 180, 90, 0, -90 };
        _rotations[3] = new int[] { 180, 108, 36, -36, -108 };
    }


    bool _rotating;

    public bool Rotating => _rotating;

    public void RotateMaskToCamera(int index)
    {
        _rotating = true;
        int desiredAngle = _rotations[_masks.Count - 2][index];

        Sequence rotateSequence = DOTween.Sequence();
        rotateSequence.Append(transform.DORotate(desiredAngle * Vector3.up, 0.35f))
            .OnComplete(() => _rotating = false);
    }
}

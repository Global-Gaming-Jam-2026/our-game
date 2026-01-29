using UnityEngine;
using DG.Tweening;

public class BullAttack : StateBase
{
    [SerializeField] GameObject[] _chargeStartPositions;

    Vector2 _chargeStartPos;
    Vector2 _chargeEndPos;

    Sequence _attackSequence;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _chargeEndPos = _chargeStartPos = ChooseStartPosition();
        _chargeEndPos.x *= -1;

        AnimateAttack();
    }

    Vector2 ChooseStartPosition()
    {
        //Go to the position opposite to the player's side of the screen
        foreach (var positionGO in _chargeStartPositions)
        {
            Vector2 position = positionGO.transform.position;
            if (Mathf.Sign(position.x) == -Mathf.Sign(PlayerController.Instance.transform.position.x))
            {
                return position;
            }
        }
        return _chargeStartPositions[0].transform.position;
    }

    void AnimateAttack()
    {
        Debug.Log("Starting bull attack!");
        Vector2 startPosition = Controller.Body.transform.position;
        _attackSequence = DOTween.Sequence();
        _attackSequence.Append(Controller.Body.DOMove(_chargeStartPos, 1.5f).SetEase(Ease.InOutSine))
            .Append(Controller.Body.DOMove(_chargeEndPos, 1.5f).SetEase(Ease.InOutSine).SetDelay(0.75f))
            .Append(Controller.Body.DOMove(startPosition, 1.5f).SetEase(Ease.InOutSine).SetDelay(0.5f))
            .OnComplete(() => _isDone = true);
        _attackSequence.Play();
    }
}

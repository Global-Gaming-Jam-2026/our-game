using UnityEngine;
using DG.Tweening;

public class BullAttack : StateBase
{
    [SerializeField] GameObject[] _chargeStartPositions;
    [SerializeField] Collider2D _playerDamageCollider;

    [Tooltip("How long it takes the bull to charge across the screen. Smaller values are faster")]
    [SerializeField][Min(0.3f)]  float _chargeDuration;

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
        _attackSequence
            .Append(Controller.Body.DOMove(_chargeStartPos, 1.5f).SetEase(Ease.InOutSine)) // moving to start position
            .Append(Controller.Body.DOMove(_chargeEndPos, _chargeDuration).SetEase(Ease.InSine).SetDelay(0.5f) // swiping across the screen after delay of 0.75 seconds
                .OnStart(() => _playerDamageCollider.gameObject.SetActive(true)) // activating player damage collider when swipe starts
                .OnComplete(() => _playerDamageCollider.gameObject.SetActive(false))) // deactivating player damage collider when swipe ends
            .Append(Controller.Body.transform.DOShakePosition(0.5f, 0.5f, 90)) // shaking a bit when swipe ends
            .Append(Controller.Body.DOMove(startPosition, 1.5f).SetEase(Ease.InOutSine).SetDelay(0.2f)) // returning to start position
            .OnComplete(() => _isDone = true); //signalling the state is complete
        _attackSequence.Play();
    }
}

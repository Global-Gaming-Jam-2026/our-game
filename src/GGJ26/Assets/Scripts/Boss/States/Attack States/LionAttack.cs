using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class LionAttack : StateBase
{
    [SerializeField] GameObject[] _roarPositions;
    [SerializeField] GameObject _roarIndicator;
    [SerializeField] Collider2D _roarCollider;

    [SerializeField] AudioClip _indicatorSFX;
    [SerializeField] AudioClip[] _roarLibrary;

    [Tooltip("The time between the head arriving at its location and the red indicator appearing")]
    [SerializeField] float _indicatorAppearDelay;

    [Tooltip("How long the reed indicator stays")]
    [SerializeField] float _roarChargeUpTime;

    [Tooltip("How long the AOE lasts")]
    [SerializeField] float _roarDuration;

    Sequence _moveToRoarPosition;
    Vector2 _chosenRoarPosition;

    public override void OnStateEnter()
    {
        Debug.Log("Starting Lion Attack");
        base.OnStateEnter();

        _chosenRoarPosition = ChooseRoarPosition();
        AnimateRoar();
    }

    private void AnimateRoar()
    {
        Vector2 position = Controller.Body.transform.position;
        _moveToRoarPosition = DOTween.Sequence();
        _moveToRoarPosition
            .Append(Controller.Body.DOMove(_chosenRoarPosition, 1f * CooldownHolder.GlobalCooldownMultiplier).SetEase(Ease.InOutExpo))
            .Append(Controller.Body.transform.DOShakePosition((_roarChargeUpTime + _indicatorAppearDelay) * CooldownHolder.GlobalCooldownMultiplier, 0.4f, 60)
                .OnStart(() =>
                {
                    DOVirtual.DelayedCall(_indicatorAppearDelay * CooldownHolder.GlobalCooldownMultiplier, () =>
                    {
                        Controller.SFXPlayer.PlaySFX(_indicatorSFX);
                        _roarIndicator.transform.position = Controller.Body.transform.position;
                        _roarIndicator.gameObject.SetActive(true);
                    });
                })
                .OnComplete(() =>
                {
                    Controller.SFXPlayer.PlaySFX(_roarLibrary[Random.Range(0, _roarLibrary.Length)]);
                    _roarIndicator.SetActive(false);
                    _roarCollider.transform.position = Controller.Body.transform.position;
                    _roarCollider.gameObject.SetActive(true);
                }))
            .Append(Controller.Body.DOMove(position, 1.5f * CooldownHolder.GlobalCooldownMultiplier).SetEase(Ease.InOutExpo).SetDelay(_roarDuration)
                .OnStart(() => _roarCollider.gameObject.SetActive(false))
                .OnComplete(() => _isDone = true));
    }

    Vector2 ChooseRoarPosition()
    {
        List<Vector2> possiblePositions = new List<Vector2>();
        foreach (var positionGO in _roarPositions)
        {
            Vector2 position = positionGO.transform.position;

            //chooses the two possible positions on the same screen side as the player
            if (Mathf.Sign(position.x) == Mathf.Sign(PlayerController.Instance.transform.position.x))
            {
                possiblePositions.Add(position);
            }
        }

        int randomIndex = Random.Range(0, possiblePositions.Count);
        return possiblePositions[randomIndex];
    }
}

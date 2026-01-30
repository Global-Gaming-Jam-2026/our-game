using DG.Tweening;
using UnityEngine;

public class BossDeathState : StateBase
{
    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Sequence deathSequence = DOTween.Sequence();

        Vector3 targetPosition = Controller.Body.transform.parent.position - 10 * Vector3.up;
        deathSequence.Append(Controller.Body.transform.parent.DOMove(targetPosition, 5.5f).SetEase(Ease.Linear))
            .Join(Controller.Body.transform.DOShakePosition(7f, Vector3.right, 20));

        deathSequence.Play();
    }
}

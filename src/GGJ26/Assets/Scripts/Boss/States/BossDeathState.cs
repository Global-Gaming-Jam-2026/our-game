using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class BossDeathState : StateBase
{
    [SerializeField] private string _winSceneName = "WinScene"; 

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Sequence deathSequence = DOTween.Sequence();

        Vector3 targetPosition = Controller.Body.transform.parent.position - 10 * Vector3.up;

        deathSequence.Append(Controller.Body.transform.parent.DOMove(targetPosition, 5.5f).SetEase(Ease.Linear))
            .Join(Controller.Body.transform.DOShakePosition(7f, Vector3.right, 20));

        deathSequence.OnComplete(() =>
        {
            SceneManager.LoadScene(_winSceneName);
        });

        deathSequence.Play();
    }
}
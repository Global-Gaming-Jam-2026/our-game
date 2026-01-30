using DG.Tweening;
using UnityEngine;

public class BirdAttack : StateBase
{
    [Header("Animation")]
    [SerializeField] GameObject[] _positions;

    [Header("Projectiles")]
    [SerializeField] ProjectileParent _projectileParent;
    [SerializeField] [Min(0.2f)] float _projectileInterval;

    Vector2 _startTargetPosition, _startPosition;
    float _shootTime;

    public override void OnStateEnter()
    {
        Debug.Log("Starting bird attack");
        base.OnStateEnter();
        _startTargetPosition = ChooseRandomTargetPosition();
        _startPosition = Controller.Body.position;
        _shootTime = Time.time - _projectileInterval / 2f;
    }

    private Vector2 ChooseRandomTargetPosition()
    {
        int randomIndex = Random.Range(0, _positions.Length);
        return _positions[randomIndex].transform.position;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (StateTime < 6f)
        {
            AnimateEightFigure();
            SpawnProjectiles();
        }
        else
        {
            MoveBack();
        }
    }

    private void SpawnProjectiles()
    {
        if (Time.time >= _shootTime + _projectileInterval)
        {
            Projectile nextProj = _projectileParent.GetNextProjectile();
            if (nextProj != null)
            {
                nextProj.transform.position = Controller.Body.transform.position;
                nextProj.gameObject.SetActive(true);
            }
            _shootTime = Time.time;
        }
    }

    private void AnimateEightFigure()
    {
        Vector2 position = new Vector2();
        position.x = Mathf.Sin(StateTime) * _startTargetPosition.x;
        position.y = _startPosition.y + Mathf.Sin(StateTime * 2f);
        Controller.Body.position = position;
    }

    private void MoveBack()
    {
        Controller.Body.position = Vector2.Lerp(Controller.Body.position, _startPosition, 8f * Time.deltaTime);
        if (Vector2.Distance(Controller.Body.position, _startPosition) <= 0.2f)
        {
            Controller.Body.position = _startPosition;
            _isDone = true;
        }
    }
}

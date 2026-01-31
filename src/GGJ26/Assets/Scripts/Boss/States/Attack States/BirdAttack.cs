using DG.Tweening;
using UnityEngine;

public class BirdAttack : StateBase
{
    [SerializeField] AudioClip[] _birdScremLibrary;
    [SerializeField] float _sfxCooldown;

    [Header("Animation")]
    [SerializeField] GameObject[] _positions;

    [Header("Projectiles")]
    [SerializeField] ProjectileParent _projectileParent;
    [SerializeField] [Min(0.2f)] float _projectileInterval;

    Vector2 _startTargetPosition, _startPosition;
    float _shootTime;
    float _localCooldownMultiplier;
    float _sfxCooldownCounter;

    public override void OnStateEnter()
    {
        Debug.Log("Starting bird attack");
        base.OnStateEnter();
        _sfxCooldownCounter = 0f;
        _localCooldownMultiplier = Mathf.Max(CooldownHolder.GlobalCooldownMultiplier, 0.6f);
        _startTargetPosition = ChooseRandomTargetPosition();
        _startPosition = Controller.Body.position;
        _shootTime = Time.time - _projectileInterval * _localCooldownMultiplier / 2f;
    }

    private Vector2 ChooseRandomTargetPosition()
    {
        int randomIndex = Random.Range(0, _positions.Length);
        return _positions[randomIndex].transform.position;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        HandleSFX();
        AnimateBird();
    }

    private void AnimateBird()
    {
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

    private void HandleSFX()
    {
        if (!Controller.SFXPlayer.IsPlaying && _sfxCooldownCounter <= 0)
        {
            _sfxCooldownCounter = _sfxCooldown;
            Controller.SFXPlayer.PlaySFX(_birdScremLibrary[Random.Range(0, _birdScremLibrary.Length)]);
        }
        else
        {
            _sfxCooldownCounter -= Time.deltaTime;
        }
    }

    private void SpawnProjectiles()
    {
        if (Time.time >= _shootTime + _projectileInterval * _localCooldownMultiplier)
        {
            DamageCollider nextProj = _projectileParent.GetNextProjectile();
            if (nextProj != null)
            {
                nextProj.transform.position = Controller.Body.transform.position;
                nextProj.gameObject.SetActive(true);
                nextProj.GetComponent<Rigidbody2D>().linearVelocity =
                    GamePhysics.InitialVelocityFromTarget(
                        Controller.Body.transform.position, 
                        PlayerController.Instance.transform.position + 0.5f * Vector3.up, 
                        0.5f, 
                        Physics2D.gravity);
            }
            _shootTime = Time.time;
        }
    }

    private void AnimateEightFigure()
    {
        Vector2 position = new Vector2();
        position.x = Mathf.Sin(StateTime / _localCooldownMultiplier) * _startTargetPosition.x;
        position.y = _startPosition.y + Mathf.Sin(StateTime * 2f / _localCooldownMultiplier);
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

    private void OnEnable()
    {
        GetComponentInParent<BossAttackState>().RegisterNewAttack(this, 5, Mask.Bird);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();
        Controller.SFXPlayer.Stop();
    }
}

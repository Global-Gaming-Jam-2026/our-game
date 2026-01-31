using System.Collections;
using UnityEngine;

public class BossController : Entity
{
    [SerializeField] Collider2D _bossCollider;

    [Header("States")]
    [SerializeField] BossIdleState _idleState;
    [SerializeField] BossAttackState _attackState;
    [SerializeField] BossDeathState _deathState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isOperating = true;
        InitMachine();
        _machine.SetState(_idleState, true);

        EventBus.Instance.OnBossDefeat += Die;
        EventBus.Instance.OnCameraFlipEnd += ReenablBoss;
    }

    private void Update()
    {
        if (_isOperating)
        {
            SelectState();
        }
    }

    void Die()
    {
        _machine.SetState(_deathState, true);
        _isOperating = false;
        Destroy(_machine.gameObject);
    }

    void SelectState()
    {
        if (_machine.State == _idleState)
        {
            _machine.SetState(_attackState);
        }
        else
        {
            _machine.SetState(_idleState);
        }
    }

    public void TriggerScreenFlip()
    {
        StartCoroutine(ScreenFlipSequence());
    }

    IEnumerator ScreenFlipSequence()
    {
        while (_machine.State != _idleState)
        {
            yield return new WaitForEndOfFrame();
        }

        _isOperating = false;
        EventBus.Instance.OnCameraFlipStart?.Invoke();
    }

    void ReenablBoss()
    {
        _isOperating = true;
        _bossCollider.enabled = true;
    }
}

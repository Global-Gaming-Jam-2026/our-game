using UnityEngine;
using DG.Tweening;

public class SlamAttack : StateBase
{
    [Header("Sub States")]
    [SerializeField] TelegraphPhase _telegraphPhase;
    [SerializeField] SlamPhase _slamPhase;

    private Vector3 _originalPosition;
    
    private Sequence _slamSequence;

    public override void OnStateEnter()
    {
        _telegraphPhase.gameObject.SetActive(true);
        _slamPhase.gameObject.SetActive(true);

        base.OnStateEnter();
        Debug.Log("Starting slam attack!");

        _originalPosition = Controller.Body.transform.position;

        _subState = _telegraphPhase;
        _subState.SetController(Controller);
        _subState.OnStateEnter();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (_subState == _telegraphPhase && _subState.IsDone)
        {
            _subState.OnStateExit();
            _subState = _slamPhase;
            _slamPhase.OriginalPosition = _originalPosition;
            _slamPhase.SetController(Controller);
            _subState.OnStateEnter();
        }
        else if (_subState == _slamPhase && _subState.IsDone)
        {
            _isDone = true;
        }
    }

    private void OnEnable()
    {
        GetComponentInParent<BossAttackState>().RegisterNewAttack(this, 9);
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        _slamSequence?.Kill();
    }
}

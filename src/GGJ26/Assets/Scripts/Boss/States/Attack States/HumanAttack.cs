using UnityEngine;

public class HumanAttack : StateBase
{
    [SerializeField] SpearParent _spearParent;
    [SerializeField] AudioClip _attackSFX;
    [SerializeField] [Min(1.0f)] float _spearDistance;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Controller.SFXPlayer.PlaySFX(_attackSFX);
        _spearParent.SpawnSpears(5, _spearDistance);
    }

    public override void OnStateUpdate()
    {
        _isDone = _spearParent.LaunchedSpears && _spearParent.transform.childCount == 10;
    }

    private void OnEnable()
    {
        GetComponentInParent<BossAttackState>().RegisterNewAttack(this, 7, Mask.Human);
    }

    public override void OnStateExit()
    {
        Controller.SFXPlayer.Stop();
    }
}

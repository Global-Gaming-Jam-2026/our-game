using UnityEngine;

public class HumanAttack : StateBase
{
    [SerializeField] SpearParent _spearParent;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _spearParent.SpawnSpears(5);
    }

    public override void OnStateUpdate()
    {
        _isDone = _spearParent.LaunchedSpears && _spearParent.transform.childCount == 10;
    }

    private void OnEnable()
    {
        GetComponentInParent<BossAttackState>().RegisterNewAttack(this, 7, Mask.Human);
    }
}

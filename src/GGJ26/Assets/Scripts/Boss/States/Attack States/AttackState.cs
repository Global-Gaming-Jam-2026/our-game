using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackState : StateBase
{
    List<StateBase> _attackStates;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _subState = null;
        _attackStates = GetComponentsInChildren<StateBase>(true).ToList();
        _attackStates.Remove(this);
        ChooseRandomAttack();
    }

    void ChooseRandomAttack()
    {
        int randomIndex = Random.Range(0, _attackStates.Count);
        _subState = _attackStates[randomIndex];
        _subState.OnStateEnter();
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (_subState.IsDone)
        {
            _isDone = true;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackState : StateBase
{
    List<StateBase> _attackStates;
    int[] _attackWeights;

    private void Start()
    {
        SetupSubStates();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _subState = null;
        
        ChooseRandomAttack();
    }

    private void SetupSubStates()
    {
        _attackStates = GetComponentsInChildren<StateBase>().ToList();
        _attackStates.Remove(this);
        _attackWeights = new int[_attackStates.Count];
        for (int i = 0; i < _attackStates.Count; i++)
        {
            _attackWeights[i] = 1;
        }
    }

    void ChooseRandomAttack()
    {
        int randomIndex = WeightRandomSelector.ChooseRandomFromWeightedValues(_attackWeights);
        RebalanceWeights(randomIndex);
        _subState = _attackStates[randomIndex];
        _subState.OnStateEnter();
    }

    void RebalanceWeights(int indexToExclude)
    {
        for (int i = 0; i < _attackWeights.Length; i++)
        {
            if (i == indexToExclude)
                continue;
            _attackWeights[i]++;
        }
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

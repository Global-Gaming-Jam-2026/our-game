using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Mask
{
    Bird,
    Human,
    Bear
}

public class BossAttackState : StateBase
{
    [System.Serializable]
    class MaskToGameobject
    {
        public Mask mask;
        public GameObject maskObject;
    }

    [SerializeField] List<MaskToGameobject> _maskLibrary;
    [SerializeField] MaskRotator _maskRotator;
    
    List<StateBase> _attackStates;
    List<int> _attackWeights;

    Mask _nextMask;
    bool _queueNewMask;

    private void Start()
    {
        SetupSubStates();
    }

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _subState = null;
        
        if (_queueNewMask)
        {
            StartCoroutine(AddNewMask());
        }
        else
        {

            StartCoroutine(ChooseRandomAttack());
        }
    }

    private void SetupSubStates()
    {
        _attackStates = GetComponentsInChildren<StateBase>().ToList();
        _attackStates.Remove(this);
        _attackWeights = new List<int>();
        for (int i = 0; i < _attackStates.Count; i++)
        {
            _attackWeights.Add(1);
        }
    }

    IEnumerator ChooseRandomAttack()
    {
        int randomIndex = WeightRandomSelector.ChooseRandomFromWeightedValues(_attackWeights);
        RebalanceWeights(randomIndex);

        _maskRotator.RotateMaskToCamera(randomIndex);
        while (_maskRotator.Rotating)
        {
            yield return null;
        }

        _subState = _attackStates[randomIndex];
        _subState.OnStateEnter();
    }

    void RebalanceWeights(int indexToExclude)
    {
        for (int i = 0; i < _attackWeights.Count; i++)
        {
            if (i == indexToExclude)
                continue;
            _attackWeights[i]++;
        }
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
        if (_subState != null)
        {
            if (_subState.IsDone)
            {
                _isDone = true;
            }
        }
    }

    public void RegisterNewAttack(StateBase newAttackState, int newWeight, Mask mask)
    {
        if (!_attackStates.Contains(newAttackState))
        {
            _attackStates.Add(newAttackState);
            _attackWeights.Add(newWeight);
            newAttackState.SetController(Controller);
            _nextMask = mask;
            _queueNewMask = true;
        }
    }

    IEnumerator AddNewMask()
    {
        //Do the "insert mask" animation, and then...
        MaskToGameobject data = _maskLibrary.FirstOrDefault(pair => pair.mask == _nextMask);
        if (data != null)
        {
            GameObject maskGameObject = data.maskObject;
            _maskRotator.AddMask(maskGameObject);
            maskGameObject.SetActive(true);
        }
        while (_maskRotator.InArrangement)
        {
            yield return new WaitForEndOfFrame();
        }
        _queueNewMask = false;
        StartCoroutine(ChooseRandomAttack());
    }
}

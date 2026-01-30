using UnityEngine;

public class HumanAttack : StateBase
{
    [SerializeField] SpearParent _spearParent;

    int _spearsToSpawn;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        _spearParent.SpawnSpears(5);
        _spearParent.transform.position = PlayerController.Instance.transform.position + 0.2f * Vector3.up;
    }

    public override void OnStateUpdate()
    {
        _spearParent.transform.position =
            Vector2.Lerp(_spearParent.transform.position, PlayerController.Instance.transform.position + 0.2f * Vector3.up, 10f * Time.deltaTime);

        _isDone = _spearParent.LaunchedSpears && _spearParent.transform.childCount == 10;
    }
}

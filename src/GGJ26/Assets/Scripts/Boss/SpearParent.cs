using DG.Tweening;
using UnityEngine;

public class SpearParent : MonoBehaviour
{
    [SerializeField] Spear[] _spears;
    bool _launchedSpears;

    public bool LaunchedSpears => _launchedSpears;

    private void Start()
    {
        _spears = GetComponentsInChildren<Spear>(true);
    }

    public void SpawnSpears(int amount)
    {
        _launchedSpears = false;
        float angleInterval = 180f / (amount - 1);
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPosition = Quaternion.Euler(Vector3.forward * angleInterval * i) * Vector2.right * 5f;
            Quaternion spawnRotation = Quaternion.Euler(Vector3.forward * angleInterval * i);
            Spear nextSpear = GetNextSpear();
            nextSpear.transform.SetLocalPositionAndRotation(spawnPosition, spawnRotation);
            nextSpear.gameObject.SetActive(true);
            nextSpear.LaunchDelayed(i);
        }
        DOVirtual.DelayedCall(1.6f, () => _launchedSpears = true);
    }

    Spear GetNextSpear()
    {
        foreach (var spear in _spears)
        {
            if (!spear.gameObject.activeInHierarchy)
            {
                return spear;
            }
        }

        return _spears[_spears.Length - 1];
    }
}

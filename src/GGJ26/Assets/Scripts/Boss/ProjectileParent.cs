using UnityEngine;

public class ProjectileParent : MonoBehaviour
{
    DamageCollider[] projectiles;

    private void Start()
    {
        projectiles = GetComponentsInChildren<DamageCollider>(true);
    }

    public DamageCollider GetNextProjectile()
    {
        for (int i = 0; i < projectiles.Length; i++)
        {
            if (projectiles[i] != null && !projectiles[i].gameObject.activeInHierarchy)
            {
                return projectiles[i];
            }
        }
        return null;
    }
}

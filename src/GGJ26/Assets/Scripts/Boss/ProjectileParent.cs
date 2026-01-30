using UnityEngine;

public class ProjectileParent : MonoBehaviour
{
    Projectile[] projectiles;

    private void Start()
    {
        projectiles = GetComponentsInChildren<Projectile>(true);
    }

    public Projectile GetNextProjectile()
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

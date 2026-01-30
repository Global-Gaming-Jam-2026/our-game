using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask _collidingLayers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GamePhysics.IsInLayerMask(collision.gameObject, _collidingLayers))
        {
            gameObject.SetActive(false);
        }
    }
}

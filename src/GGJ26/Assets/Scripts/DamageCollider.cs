using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] LayerMask _collidingLayers;
    [SerializeField] int _damageDealt;
    [SerializeField] bool _disappearsWhenHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GamePhysics.IsInLayerMask(collision.gameObject, _collidingLayers))
        {
            gameObject.SetActive(!_disappearsWhenHit);
            if (collision.gameObject.TryGetComponent(out HealthModule health))
            {
                health.TakeDamage(_damageDealt);
            }
        }
    }
}

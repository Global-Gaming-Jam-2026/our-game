using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    [SerializeField] LayerMask _collidingLayers;
    [SerializeField] int _damageDealt;
    [SerializeField] bool _disappearsWhenHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GamePhysics.IsInLayerMask(collision.gameObject, _collidingLayers))
            return;
        gameObject.SetActive(!_disappearsWhenHit);

        if (collision.gameObject.name != "Player" && collision.gameObject.name != "Body")
            return;

        HealthModule health;
        if (collision.gameObject.TryGetComponent(out health))
        {
            health.TakeDamage(_damageDealt);
        }
        else
        {
            health = collision.gameObject.GetComponentInParent<HealthModule>();
            if (health != null)
            {
                health.TakeDamage(_damageDealt);
            }
        }
    }
}

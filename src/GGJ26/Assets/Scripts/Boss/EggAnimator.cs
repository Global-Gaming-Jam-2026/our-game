using UnityEngine;

public class EggAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer _eggWhiteRenderer;
    [SerializeField] Rigidbody2D _eggRB;

    [SerializeField] GameObject _splatObject;

    private void Update()
    {
        _eggWhiteRenderer.transform.up = -_eggRB.linearVelocity.normalized;
    }

    private void OnDisable()
    {
        Vector2 spawnPoint = transform.position;
        if (transform.position.y + 4f > 1f)
            return;
        spawnPoint.y = -4f;
        GameObject splatEffect = Instantiate(_splatObject, spawnPoint, Quaternion.identity);
        Destroy(splatEffect.gameObject, 2f);
    }
}

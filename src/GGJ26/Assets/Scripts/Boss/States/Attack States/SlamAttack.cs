using UnityEngine;
using DG.Tweening;

public class SlamAttack : StateBase
{
    [Header("Slam Settings")]
    [SerializeField] private float riseHeight = 3f;
    [SerializeField] private float telegraphDuration = 1.5f;
    [SerializeField] private float slamSpeed = 0.15f;
    [SerializeField] private float returnSpeed = 0.5f;

    [Header("Damage")]
    [SerializeField] private Collider2D playerDamageCollider;
    [SerializeField] private float damageActiveDuration = 0.2f;

    [Header("Visual")]
    [SerializeField] private Color shadowColor = new Color(0.3f, 0f, 0f, 0.5f);
    [SerializeField] private float shadowRadius = 2f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private GameObject shadowIndicator;
    private SpriteRenderer shadowRenderer;
    private Sequence slamSequence;

    public override void OnStateEnter()
    {
        base.OnStateEnter();  // CRITICAL: Resets _isDone to false
        Debug.Log("Starting slam attack!");

        originalPosition = Controller.Body.transform.position;

        // Target player's X position, keep our Y
        targetPosition = new Vector3(
            PlayerController.Instance.transform.position.x,
            originalPosition.y,
            0
        );

        CreateShadowIndicator();
        AnimateSlam();
    }

    private void CreateShadowIndicator()
    {
        shadowIndicator = new GameObject("SlamShadow");
        shadowRenderer = shadowIndicator.AddComponent<SpriteRenderer>();
        shadowRenderer.sprite = CreateCircleSprite();
        shadowRenderer.color = shadowColor;
        shadowRenderer.sortingOrder = -1;

        // Position shadow at target location on ground
        shadowIndicator.transform.position = new Vector3(
            targetPosition.x,
            originalPosition.y - 1f,
            0
        );
        shadowIndicator.transform.localScale = Vector3.zero;
    }

    private void AnimateSlam()
    {
        slamSequence = DOTween.Sequence();

        // Phase 1: Rise up while shadow grows
        slamSequence
            .Append(Controller.Body.transform.DOMoveY(
                originalPosition.y + riseHeight,
                telegraphDuration * 0.6f)
                .SetEase(Ease.OutCubic))
            .Join(shadowIndicator.transform.DOScale(
                new Vector3(shadowRadius * 2, shadowRadius * 0.6f, 1),
                telegraphDuration * 0.6f)
                .SetEase(Ease.OutCubic));

        // Phase 2: Hold at top
        slamSequence
            .AppendInterval(telegraphDuration * 0.4f);

        // Phase 3: Slam down fast
        slamSequence
            .Append(Controller.Body.transform.DOMove(targetPosition, slamSpeed)
                .SetEase(Ease.InQuint))
            .AppendCallback(OnSlamImpact);

        // Phase 4: Shake on impact
        slamSequence
            .Append(Controller.Body.transform.DOShakePosition(0.3f, 0.3f, 30));

        // Phase 5: Return to original position
        slamSequence
            .Append(Controller.Body.transform.DOMove(originalPosition, returnSpeed)
                .SetEase(Ease.OutCubic))
            .OnComplete(() => _isDone = true);  // CRITICAL: Signal completion
    }

    private void OnSlamImpact()
    {
        // Activate damage collider
        if (playerDamageCollider != null)
        {
            playerDamageCollider.gameObject.SetActive(true);
            DOVirtual.DelayedCall(damageActiveDuration, () =>
                playerDamageCollider.gameObject.SetActive(false));
        }

        // Hide shadow
        if (shadowIndicator != null)
        {
            shadowIndicator.SetActive(false);
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        // Kill any running sequence
        slamSequence?.Kill();

        // Cleanup shadow
        if (shadowIndicator != null)
        {
            Destroy(shadowIndicator);
            shadowIndicator = null;
        }

        // Ensure damage collider is off
        if (playerDamageCollider != null)
        {
            playerDamageCollider.gameObject.SetActive(false);
        }
    }

    private Sprite CreateCircleSprite()
    {
        int size = 32;
        Texture2D tex = new Texture2D(size, size);
        Color[] colors = new Color[size * size];

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float radius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                colors[y * size + x] = dist < radius ? Color.white : Color.clear;
            }
        }

        tex.SetPixels(colors);
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}

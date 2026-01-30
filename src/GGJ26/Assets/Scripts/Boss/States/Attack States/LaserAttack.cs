using UnityEngine;
using DG.Tweening;

public class LaserAttack : StateBase
{
    [Header("Laser Settings")]
    [SerializeField] private float laserLength = 15f;
    [SerializeField] private float laserWidth = 0.5f;
    [SerializeField] private float telegraphDuration = 2f;
    [SerializeField] private float sweepDuration = 3.5f;
    [SerializeField] private float sweepAngle = 90f;

    [Header("Damage")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damageInterval = 0.3f;

    [Header("Visual")]
    [SerializeField] private Color telegraphColor = new Color(1f, 0f, 0f, 0.3f);
    [SerializeField] private Color laserColor = new Color(1f, 0.3f, 0.1f, 1f);

    private GameObject laserObject;
    private LineRenderer lineRenderer;
    private float currentAngle;
    private bool isFiring;
    private float lastDamageTime;
    private Sequence laserSequence;

    public override void OnStateEnter()
    {
        base.OnStateEnter();
        Debug.Log("Starting laser attack!");

        isFiring = false;
        lastDamageTime = -damageInterval;

        CreateLaserVisual();
        AnimateLaser();
    }

    private void CreateLaserVisual()
    {
        laserObject = new GameObject("Laser");
        laserObject.transform.SetParent(Controller.Body.transform);
        laserObject.transform.localPosition = Vector3.zero;

        lineRenderer = laserObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.startColor = telegraphColor;
        lineRenderer.endColor = telegraphColor;
        lineRenderer.useWorldSpace = true;

        // Use default sprite material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.sortingOrder = 15;
    }

    private void AnimateLaser()
    {
        // Calculate start angle (towards player)
        Vector2 toPlayer = PlayerController.Instance.transform.position - Controller.Body.transform.position;
        float centerAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        float startAngle = centerAngle + sweepAngle / 2;
        float endAngle = centerAngle - sweepAngle / 2;

        currentAngle = startAngle;

        laserSequence = DOTween.Sequence();

        // Telegraph phase - show thin line
        laserSequence
            .AppendInterval(telegraphDuration);

        // Start firing - thicken beam
        laserSequence
            .AppendCallback(() => StartFiring());

        // Sweep across the arc
        laserSequence
            .Append(DOTween.To(
                () => currentAngle,
                x => currentAngle = x,
                endAngle,
                sweepDuration)
                .SetEase(Ease.Linear));

        // Complete
        laserSequence
            .OnComplete(() => _isDone = true);
    }

    private void StartFiring()
    {
        isFiring = true;
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth * 0.8f;
        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;
    }

    public override void OnStateUpdate()
    {
        base.OnStateUpdate();

        UpdateLaserVisual();

        if (isFiring)
        {
            CheckDamage();
        }
    }

    private void UpdateLaserVisual()
    {
        if (lineRenderer == null) return;

        Vector2 direction = new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad),
            Mathf.Sin(currentAngle * Mathf.Deg2Rad)
        );

        Vector3 start = Controller.Body.transform.position;
        Vector3 end = start + (Vector3)(direction * laserLength);

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    private void CheckDamage()
    {
        if (Time.time - lastDamageTime < damageInterval) return;

        Vector2 direction = new Vector2(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad),
            Mathf.Sin(currentAngle * Mathf.Deg2Rad)
        );

        RaycastHit2D hit = Physics2D.Raycast(
            Controller.Body.transform.position,
            direction,
            laserLength,
            playerLayer
        );

        if (hit.collider != null)
        {
            Debug.Log("Laser hit player!");
            // When health system exists: hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(12f);
            lastDamageTime = Time.time;
        }
    }

    public override void OnStateExit()
    {
        base.OnStateExit();

        laserSequence?.Kill();

        if (laserObject != null)
        {
            Destroy(laserObject);
            laserObject = null;
            lineRenderer = null;
        }

        isFiring = false;
    }
}

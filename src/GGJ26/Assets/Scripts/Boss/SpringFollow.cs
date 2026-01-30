using UnityEngine;

public class SpringFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform targetRigidbody; // The rigidbody to follow
    public Vector2 offset; // Fixed position offset from target

    [Header("Spring Settings")]
    [Range(1f, 100f)]
    public float springStrength = 50f; // How strong the spring pulls
    [Range(0f, 10f)]
    public float damping = 5f; // Reduces oscillation

    private Vector2 velocity;

    void FixedUpdate()
    {
        // Calculate desired position
        Vector2 targetPosition = (Vector2)targetRigidbody.position + offset;
        Vector2 currentPosition = transform.position;

        // Calculate spring force
        Vector2 displacement = targetPosition - currentPosition;
        Vector2 springForce = displacement * springStrength;

        // Apply damping to reduce oscillation
        Vector2 dampingForce = velocity * damping;

        // Update velocity and position
        velocity += (springForce - dampingForce) * Time.fixedDeltaTime;
        transform.position = currentPosition + velocity * Time.fixedDeltaTime;
    }
}
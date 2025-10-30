using UnityEngine;
using UnityEngine.Events;

public class HarpoonProjectile : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Fish> OnHitTarget;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private TrailRenderer projectileTrail;
    private Vector2 launchDirection;
    private bool isLaunched = false;

    public void Launch(Vector2 direction, float speed)
    {
        if (isLaunched) return;

        launchDirection = direction.normalized;
        isLaunched = true;
        rb.linearVelocity = launchDirection * speed;
        projectileTrail.emitting = true;
    }

    public void ResetProjectile(Transform transformPosition)
    {
        isLaunched = false;
        transform.position = transformPosition.position;
        projectileTrail.emitting = false;
        projectileTrail.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fish fish = collision.GetComponent<Fish>();

        rb.linearVelocity = Vector2.zero;
        OnHitTarget.Invoke(fish);
    }
}

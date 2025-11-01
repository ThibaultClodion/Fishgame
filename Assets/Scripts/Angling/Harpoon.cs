using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private HarpoonProjectile projectile;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private float projectileSpeed;
    private bool isLaunching = false;

    [Header("References")]
    [SerializeField] private GameObject target;
    [SerializeField] private float angleLimit;
    private float currentAngle;
    private LayerMask fishMask = 1 << 6;

    public void Initialize()
    {
        target.SetActive(true);
        projectile.OnHitTarget.AddListener(ProjectileHit);
    }

    public void Reset()
    {
        // Reset harpoon projectile
        isLaunching = false;
        projectile.ResetProjectile(launchPoint);

        // Reset harpoon rotation and target
        target.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Rotate(Vector2 aimingDirection)
    {
        if(isLaunching) return;

        currentAngle = Vector2.SignedAngle(Vector2.down, aimingDirection);
        currentAngle = Mathf.Clamp(currentAngle, -angleLimit, angleLimit);

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    public void Shoot()
    {
        isLaunching = true;
        target.SetActive(false);

        Vector2 aimingDirection = Quaternion.Euler(0, 0, currentAngle) * Vector2.down;

        projectile.Launch(aimingDirection, projectileSpeed);
    }

    private void ProjectileHit(Fish fish)
    {
        if (fish == null)
        {
            projectile.ResetProjectile(launchPoint);
            isLaunching = false;
        }
        else
        {
            GamepadVibration.Instance.Vibration(0.5f, 0.5f, 0.2f);
            fish.Hook();
            GameManager.Instance.HookFish(fish);
        }
    }
}

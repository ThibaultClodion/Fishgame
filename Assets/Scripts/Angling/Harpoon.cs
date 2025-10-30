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

        float angle = Vector2.SignedAngle(Vector2.down, aimingDirection);

        angle = Mathf.Clamp(angle, -angleLimit, angleLimit);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public Fish Shoot(Vector2 aimingDirection)
    {
        isLaunching = true;
        target.SetActive(false);
        projectile.Launch(aimingDirection, projectileSpeed);

        return null;
    }

    private void ProjectileHit(Fish fish)
    {
        if(isLaunching == false) return;

        if (fish == null)
        {
            projectile.ResetProjectile(launchPoint);
            isLaunching = false;
        }
        else
        {
            fish.Hook();
            GameManager.Instance.HookFish(fish);
        }
    }
}

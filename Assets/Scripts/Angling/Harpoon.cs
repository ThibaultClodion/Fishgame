using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float angleLimit;
    private LayerMask fishMask = 1 << 6;

    public void Initialize()
    {
        target.SetActive(true);
    }

    public void Reset()
    {
        target.SetActive(false);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Rotate(Vector2 aimingDirection)
    {
        float angle = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg + 90;

        if (Mathf.Abs(angle) - angleLimit > 0)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Shoot(Vector2 aimingDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimingDirection, Mathf.Infinity, fishMask);

        if (hit.collider != null)
        {
            FishData fishData = hit.collider.GetComponent<Fish>().Catch();
        }

        Reset();
    }
}

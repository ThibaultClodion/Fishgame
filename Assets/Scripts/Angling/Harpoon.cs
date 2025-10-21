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
        float angle = Vector2.SignedAngle(Vector2.down, aimingDirection);

        angle = Mathf.Clamp(angle, -angleLimit, angleLimit);

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public bool Shoot(Vector2 aimingDirection)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimingDirection, Mathf.Infinity, fishMask);

        if (hit.collider != null)
        {
            FishData fishData = hit.collider.GetComponent<Fish>().Catch(Vector3.zero);
            GameManager.Instance.CatchFish(fishData);
            return true;
        }

        return false;
    }
}

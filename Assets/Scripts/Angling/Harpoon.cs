using UnityEngine;

public class Harpoon : MonoBehaviour
{
    [SerializeField] private float angleLimit;

    public void Rotate(Vector2 aimingDirection)
    {
        float angle = Mathf.Atan2(aimingDirection.y, aimingDirection.x) * Mathf.Rad2Deg + 90;

        if (Mathf.Abs(angle) - angleLimit > 0)
        {
            return;
        }

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}

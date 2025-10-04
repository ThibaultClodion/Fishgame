using UnityEngine;
using UnityEngine.InputSystem;

public class Angler : MonoBehaviour
{
    [SerializeField] private Harpoon harpoon;
    [SerializeField] private DynamicKeyImage keyImage;

    private Vector2 aimingDirection;

    private void OnEnable()
    {
        InputManager.Instance.LeftJoystick.started += StartAngling;
        InputManager.Instance.LeftJoystick.performed += UpdateAngling;
        InputManager.Instance.LeftJoystick.canceled += CancelAngling;

        InputManager.Instance.RightTrigger.performed += LaunchHarpoon;
    }

    private void OnDisable()
    {
        InputManager.Instance.LeftJoystick.started -= StartAngling;
        InputManager.Instance.LeftJoystick.performed -= UpdateAngling;
        InputManager.Instance.LeftJoystick.canceled -= CancelAngling;

        InputManager.Instance.RightTrigger.performed -= LaunchHarpoon;
    }

    private void StartAngling(InputAction.CallbackContext ctx)
    {
        keyImage.gameObject.SetActive(true);
    }

    private void UpdateAngling(InputAction.CallbackContext ctx)
    {
        aimingDirection = ctx.ReadValue<Vector2>();
        harpoon.Rotate(aimingDirection);
    }

    private void CancelAngling(InputAction.CallbackContext ctx)
    {
        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
        harpoon.Reset();
    }

    private void LaunchHarpoon(InputAction.CallbackContext ctx)
    {
        harpoon.Shoot(aimingDirection);
        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Angler : MonoBehaviour
{
    [SerializeField] private Harpoon harpoon;
    [SerializeField] private DynamicKeyImage keyImage;

    private Vector2 aimingDirection;
    private bool canAngle = false;

    // TODO: fix bug with when using mouse and starting angling for the second time (maybe use polling rather than events ?)
    // as the mouse is a bit more fucky when it comes to started events

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
        canAngle = true;
        keyImage.gameObject.SetActive(true);
        harpoon.Initialize();
    }

    private void UpdateAngling(InputAction.CallbackContext ctx)
    {
        if(!canAngle)
            return;

        aimingDirection = ctx.ReadValue<Vector2>();
        harpoon.Rotate(aimingDirection);
    }

    private void CancelAngling(InputAction.CallbackContext ctx)
    {
        canAngle = false;

        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
        harpoon.Reset();
    }

    private void LaunchHarpoon(InputAction.CallbackContext ctx)
    {
        canAngle = false;

        harpoon.Shoot(aimingDirection);
        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
    }
}

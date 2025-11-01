using UnityEngine;
using UnityEngine.InputSystem;

public class Angler : MonoBehaviour
{
    [SerializeField] private Harpoon harpoon;
    [SerializeField] private DynamicKeyImage keyImage;

    private Vector2 aimingDirection;

    private void OnEnable()
    {
        InputManager.Instance.LeftJoystick.started += UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.canceled += UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.performed += UpdateAimingDirection;
        
        InputManager.Instance.RightTrigger.performed += LaunchHarpoon;

        StartAngling();
    }

    private void OnDisable()
    {
        InputManager.Instance.LeftJoystick.started -= UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.canceled -= UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.performed -= UpdateAimingDirection;

        InputManager.Instance.RightTrigger.performed -= LaunchHarpoon;

        keyImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        harpoon.Rotate(aimingDirection);
    }

    private void UpdateAimingDirection(InputAction.CallbackContext ctx)
    {
        aimingDirection = ctx.ReadValue<Vector2>();
    }

    private void StartAngling()
    {
        keyImage.gameObject.SetActive(true);
        harpoon.Initialize();
    }

    public void CancelAngling()
    {
        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
        harpoon.Reset();
    }

    private void LaunchHarpoon(InputAction.CallbackContext ctx)
    {
        GamepadVibration.Instance.Vibration(1f, 0, 0.1f);
        harpoon.Shoot();
    }
}

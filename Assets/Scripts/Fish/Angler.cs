using UnityEngine;
using UnityEngine.InputSystem;

public class Angler : MonoBehaviour
{
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
        if(InputManager.Instance.UsingKeyboard)
        {
            // Ignore keyboard input for angling
            return;
        }

        keyImage.gameObject.SetActive(true);
    }

    private void UpdateAngling(InputAction.CallbackContext ctx)
    {
        if (InputManager.Instance.UsingKeyboard)
        {
            // Ignore keyboard input for angling
            return;
        }

        aimingDirection = ctx.ReadValue<Vector2>();
    }

    private void CancelAngling(InputAction.CallbackContext ctx)
    {
        if (InputManager.Instance.UsingKeyboard)
        {
            // Ignore keyboard input for angling
            return;
        }

        Debug.Log("Cancelling Angling!");

        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
    }

    private void LaunchHarpoon(InputAction.CallbackContext ctx)
    {
        if (InputManager.Instance.UsingKeyboard)
        {
            // Ignore keyboard input for angling
            return;
        }

        Debug.Log("Launching Harpoon! on direction " + aimingDirection);

        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
    }
}

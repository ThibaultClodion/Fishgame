using UnityEngine;
using UnityEngine.InputSystem;

public class Angler : MonoBehaviour
{
    [SerializeField] private Harpoon harpoon;
    [SerializeField] private DynamicKeyImage keyImage;

    [Header("Aiming Settings")]
    [SerializeField] private float minAimingMagnitude = 0.05f;

    private Vector2 aimingDirection;
    private bool isAngling;

    private void OnEnable()
    {
        InputManager.Instance.LeftJoystick.started += UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.canceled += UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.performed += UpdateAimingDirection;
        
        InputManager.Instance.RightTrigger.started += LaunchHarpoon;
    }

    private void OnDisable()
    {
        InputManager.Instance.LeftJoystick.started -= UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.canceled -= UpdateAimingDirection;
        InputManager.Instance.LeftJoystick.performed -= UpdateAimingDirection;

        InputManager.Instance.RightTrigger.started -= LaunchHarpoon;
    }

    private void Update()
    {
        if (aimingDirection.magnitude > minAimingMagnitude && !isAngling)
        {
            StartAngling();
        }
        else if(aimingDirection.magnitude < minAimingMagnitude && isAngling)
        {
            CancelAngling();
        }
        else
        {
            UpdateAngling();
        }
    }

    private void UpdateAimingDirection(InputAction.CallbackContext ctx)
    {
        aimingDirection = ctx.ReadValue<Vector2>();
    }

    private void StartAngling()
    {
        isAngling = true;
        keyImage.gameObject.SetActive(true);
        harpoon.Initialize();
    }

    private void UpdateAngling()
    {
        harpoon.Rotate(aimingDirection);
    }

    private void CancelAngling()
    {
        isAngling = false;
        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
        harpoon.Reset();
    }

    private void LaunchHarpoon(InputAction.CallbackContext ctx)
    {
        harpoon.Shoot(aimingDirection);
        CancelAngling();
    }
}

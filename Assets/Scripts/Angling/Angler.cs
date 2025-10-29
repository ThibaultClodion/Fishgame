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
    }

    private void Update()
    {
        UpdateAngling();
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

    private void UpdateAngling()
    {
        harpoon.Rotate(aimingDirection);
    }

    public void CancelAngling()
    {
        keyImage.gameObject.SetActive(false);
        aimingDirection = Vector2.zero;
        harpoon.Reset();
    }

    private void LaunchHarpoon(InputAction.CallbackContext ctx)
    {
        Fish fish = harpoon.Shoot(aimingDirection);

        if (fish == null)
        {
            CancelAngling();
            GameManager.Instance.StopAngling();
        }
        else
        {
            keyImage.gameObject.SetActive(false);
            GameManager.Instance.HookFish(fish);
        }
    }
}

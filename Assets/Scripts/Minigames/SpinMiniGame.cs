using UnityEngine;
using UnityEngine.InputSystem;

public class SpinMiniGame : BaseMiniGame
{
    [Header("Spin Mini Game Settings")]
    [SerializeField] private bool isClockwise = true;
    [SerializeField] private float progressionDecreaseRate = 0.05f;
    [SerializeField] private float progressionIncreaseRate = 0.0001f;

    private Vector2 lastJoystickPosition;

    public override void Initialize()
    {
        base.Initialize();

        InputManager.Instance.RightJoystick.performed += RightJoystickMove;
    }

    public override void Disable()
    {
        InputManager.Instance.RightJoystick.performed -= RightJoystickMove;
        lastJoystickPosition = Vector2.zero;
    }

    private void Update()
    {
        GetOnAddToProgression()?.Invoke(-progressionDecreaseRate * Time.deltaTime);
    }

    private void RightJoystickMove(InputAction.CallbackContext callbackContext)
    {
        // TODO : prevent cheating (e.g., quickly moving joystick back and forth)

        Vector2 joystickPosition = callbackContext.ReadValue<Vector2>();

        if(lastJoystickPosition == Vector2.zero)
        {
            lastJoystickPosition = joystickPosition;
            return;
        }

        float angleDifference = Vector2.SignedAngle(lastJoystickPosition, joystickPosition);
        lastJoystickPosition = joystickPosition;

        // Only increase progression if the joystick is moved in the correct direction
        if (isClockwise && angleDifference < 0)
        {
            GetOnAddToProgression()?.Invoke(progressionIncreaseRate * -angleDifference);
        }
        else if(!isClockwise && angleDifference > 0)
        {
            GetOnAddToProgression()?.Invoke(progressionIncreaseRate * angleDifference);
        }
    }
}

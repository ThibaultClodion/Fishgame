using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpinMiniGame : BaseMiniGame
{
    [Header("Spin Mini Game Settings")]
    [SerializeField] private float minDecreaseRate = 0.05f;
    [SerializeField] private float maxDecreaseRate = 0.15f;
    [SerializeField] private float increaseRate = 0.0003f;

    [Header("Switch Joystick Settings")]
    [Range(0,1)]
    [SerializeField] private float minDifficultyForSwitchJoystick = 0.7f;
    [SerializeField] private float minTimeBeforeSwitch = 0.5f;
    [SerializeField] private float maxTimeBeforeSwitch = 1f;

    [Header("Objet References")]
    [SerializeField] private DynamicKeyImage keyImage;
    [SerializeField] private Transform arrowTransform;

    private Vector2 lastJoystickPosition;
    private float decreaseRate;
    private bool isCurrentRightJoystick;
    private bool isCurrentClockwise;
    private bool isPlaying;

    public override void Initialize(FishData data)
    {
        base.Initialize(data);
        isPlaying = true;

        // Decrease Rate depends of fish difficulty
        decreaseRate = Mathf.Lerp(minDecreaseRate, maxDecreaseRate, data.Difficulty);

        LaunchRandomSpinInteraction();

        // Spin interaction can switch if it is a hard fish
        if (data.Difficulty >= minDifficultyForSwitchJoystick)
            StartCoroutine(ChangeSpinInteractionRoutine());
    }

    public override void Disable()
    {
        if (isCurrentRightJoystick)
            InputManager.Instance.RightJoystick.performed -= JoystickInteraction;
        else
            InputManager.Instance.LeftJoystick.performed -= JoystickInteraction;

        isPlaying = false;
        lastJoystickPosition = Vector2.zero;
    }

    private void Update()
    {
        AddToProgression(-decreaseRate * Time.deltaTime);
    }

    private IEnumerator ChangeSpinInteractionRoutine()
    {
        // Wait a random time
        float waitTime = Random.Range(minTimeBeforeSwitch, maxTimeBeforeSwitch);
        yield return new WaitForSeconds(waitTime);

        if (!isPlaying)
        {
            yield return null;
        }
        else
        {
            LaunchRandomSpinInteraction();

            // Loop the routine
            StartCoroutine(ChangeSpinInteractionRoutine());
        }
    }

    private void LaunchRandomSpinInteraction()
    {
        ChangeSpinInteraction(Random.Range(0, 2) == 0, Random.Range(0, 2) == 0);
    }

    private void ChangeSpinInteraction(bool isClockwise, bool isRightJoystick)
    {
        if(isCurrentRightJoystick)
            InputManager.Instance.RightJoystick.performed -= JoystickInteraction;
        else
            InputManager.Instance.LeftJoystick.performed -= JoystickInteraction;

        if (isRightJoystick)
            InputManager.Instance.RightJoystick.performed += JoystickInteraction;
        else
            InputManager.Instance.LeftJoystick.performed += JoystickInteraction;

        isCurrentClockwise = isClockwise;
        isCurrentRightJoystick = isRightJoystick;
        UpdateVisual();
    }

    private void JoystickInteraction(InputAction.CallbackContext callbackContext)
    {
        Vector2 joystickPosition = callbackContext.ReadValue<Vector2>();

        // First input, just store the position
        if (lastJoystickPosition == Vector2.zero)
        {
            lastJoystickPosition = joystickPosition;
            return;
        }

        float angleDifference = Vector2.SignedAngle(lastJoystickPosition, joystickPosition);
        lastJoystickPosition = joystickPosition;

        // Ignore too big movements (cheating or bad input)
        if (angleDifference > 40)
            return;

        // Only increase progression if the joystick is moved in the correct direction
        if (isCurrentClockwise && angleDifference < 0)
            AddToProgression(increaseRate * -angleDifference);
        else if(!isCurrentClockwise && angleDifference > 0)
            AddToProgression(increaseRate * angleDifference);
    }

    private void UpdateVisual()
    {
        // Change Joystick image
        if(isCurrentRightJoystick)
            keyImage.SetAction(InputManager.Instance.RightJoystick);
        else
            keyImage.SetAction(InputManager.Instance.LeftJoystick);

        // Change arrow direction
        if (isCurrentClockwise)
            arrowTransform.localEulerAngles = new Vector3(0, 180, 0);
        else
            arrowTransform.localEulerAngles = Vector3.zero;
    }
}

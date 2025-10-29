using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ZoneMiniGame : BaseMiniGame
{
    [Header("Zone Settings")]
    [SerializeField] private float minZonePortion = 0.15f;
    [SerializeField] private float maxZonePortion = 0.15f;

    [Header("Zone Speed Settings")]
    [SerializeField] private float zoneMoveSpeed = 300f;
    [SerializeField] private float gravity = 600f;
    [SerializeField] private float maxFallSpeed = 400f;
    private float currentFallSpeed = 0f;

    [Header("Progression Settings")]
    [SerializeField] private float minDecreaseRate = 0.1f;
    [SerializeField] private float maxDecreaseRate = 0.15f;
    private float decreaseRate;
    [SerializeField] private float minIncreaseRate = 0.2f;
    [SerializeField] private float maxIncreaseRate = 0.2f;
    private float increaseRate;

    [Header("Fish Settings")]
    [SerializeField] private float minFishSpeed = 0.3f;
    [SerializeField] private float maxFishSpeed = 0.5f;
    private float fishSpeed;

    [Header("References")]
    [SerializeField] private Slider fishSlider;
    [SerializeField] private RectTransform zoneTransform;
    [SerializeField] private Image fishImage;

    // Minigame variables
    private float startTime;
    private float sliderHeight;
    private float zoneHeight;
    private float minZonePos;
    private float maxZonePos;
    private bool isHoldingButton;

    public override void Initialize(FishData data)
    {
        base.Initialize(data);

        // Initialize variables
        fishImage.sprite = data.Sprite;
        startTime = Time.time;
        sliderHeight = fishSlider.GetComponent<RectTransform>().rect.height;
        zoneHeight = sliderHeight * Mathf.Lerp(minZonePortion, maxZonePortion, data.Difficulty);
        minZonePos = zoneHeight / 2;
        maxZonePos = sliderHeight - zoneHeight / 2;
        decreaseRate = Mathf.Lerp(minDecreaseRate, maxDecreaseRate, data.Difficulty);
        increaseRate = Mathf.Lerp(minIncreaseRate, maxIncreaseRate, data.Difficulty);
        fishSpeed = Mathf.Lerp(minFishSpeed, maxFishSpeed, data.Difficulty);

        // Set zone size and position
        zoneTransform.sizeDelta = new Vector2(zoneTransform.sizeDelta.x, zoneHeight);
        zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, minZonePos);

        InputManager.Instance.SouthButtonAction.started += HoldButton;
        InputManager.Instance.SouthButtonAction.canceled += ReleaseButton;
    }

    public override void Disable()
    {
        isHoldingButton = false;
        InputManager.Instance.SouthButtonAction.started -= HoldButton;
        InputManager.Instance.SouthButtonAction.canceled -= ReleaseButton;
    }

    private void Update()
    {
        FishMovement();
        ZoneMovement();
        Progression();
    }

    private void FishMovement()
    {
        // TODO : Improve fish IA movement
        fishSlider.value = Mathf.PingPong((Time.time - startTime) * fishSpeed, 1f);
    }

    private void ZoneMovement()
    {
        float newY;

        if (isHoldingButton)
        {
            currentFallSpeed = 0f;

            newY = Mathf.Min(zoneTransform.anchoredPosition.y + zoneMoveSpeed * Time.deltaTime, maxZonePos);

        }
        else
        {
            // Gravity Effect
            currentFallSpeed += gravity * Time.deltaTime;
            currentFallSpeed = Mathf.Min(currentFallSpeed, maxFallSpeed);

            newY = Mathf.Max(zoneTransform.anchoredPosition.y - currentFallSpeed * Time.deltaTime, minZonePos);
        }

        zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, newY);
    }

    private void Progression()
    {
        if (fishSlider.value * sliderHeight > zoneTransform.anchoredPosition.y - zoneHeight / 2 &&
            fishSlider.value * sliderHeight < zoneTransform.anchoredPosition.y + zoneHeight / 2)
        {
            AddToProgression(increaseRate * Time.deltaTime);
        }
        else
        {
            AddToProgression(-decreaseRate * Time.deltaTime);
        }
    }

    private void HoldButton(InputAction.CallbackContext callbackContext)
    {
        isHoldingButton = true;
    }

    private void ReleaseButton(InputAction.CallbackContext callbackContext)
    {
        isHoldingButton = false;
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ZoneMiniGame : BaseMiniGame
{
    [Header("Zone Settings")]
    [SerializeField] private float zonePortion = 0.1f;
    [SerializeField] private float zoneMoveSpeed = 0.5f;

    [Header("Progression Settings")]
    [SerializeField] private float decreaseRate = 0.1f;
    [SerializeField] private float increaseRate = 0.2f;


    [SerializeField] private Slider fishSlider;
    [SerializeField] private RectTransform zoneTransform;

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

        // TODO : Adjust settings based on fish data (difficulty ...)

        // Initialize variables
        startTime = Time.time;
        sliderHeight = fishSlider.GetComponent<RectTransform>().rect.height;
        zoneHeight = sliderHeight * zonePortion;
        minZonePos = zoneHeight / 2;
        maxZonePos = sliderHeight - zoneHeight / 2;

        // Set zone size and position
        zoneTransform.sizeDelta = new Vector2(zoneTransform.sizeDelta.x, zoneHeight);
        zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, minZonePos);

        InputManager.Instance.SouthButtonAction.started += HoldButton;
        InputManager.Instance.SouthButtonAction.canceled += ReleaseButton;
    }

    public override void Disable()
    {
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
        fishSlider.value = Mathf.PingPong((Time.time - startTime) * 0.5f, 1f);
    }

    private void ZoneMovement()
    {
        // TODO : Improve movement for a more natural feeling (gravity ...)
        if (isHoldingButton)
        {
            zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, zoneTransform.anchoredPosition.y + zoneMoveSpeed * Time.deltaTime);

            if (zoneTransform.anchoredPosition.y > maxZonePos)
                zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, maxZonePos);
        }
        else
        {
            zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, zoneTransform.anchoredPosition.y - zoneMoveSpeed * Time.deltaTime);

            if (zoneTransform.anchoredPosition.y < minZonePos)
                zoneTransform.anchoredPosition = new Vector2(zoneTransform.anchoredPosition.x, minZonePos);
        }
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

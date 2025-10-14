using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BestiaryDisplayer : MonoBehaviour
{
    [Header("Prefabs References")]
    [SerializeField] private FishType[] fishTypes;
    [SerializeField] private FishButton fishButtonPrefab;

    [Header("Scene References")]
    [SerializeField] private Transform fishButtonContainer;
    [SerializeField] private FishDisplay fishDisplay;

    private void Start()
    {
        InputManager.Instance.SelectButtonAction.performed += SwitchEnable;
    }

    private void SwitchEnable(InputAction.CallbackContext ctx)
    {
        if(fishButtonContainer.gameObject.activeSelf)
        {
            DisableFishButtons();
        }
        else
        {
            EnableFishButtons();
        }
    }

    private void EnableFishButtons()
    {
        fishButtonContainer.gameObject.SetActive(true);
        fishDisplay.gameObject.SetActive(true);

        for (int i = 0; i < fishTypes.Length; i++)
        {
            FishButton fishButton = Instantiate(fishButtonPrefab, fishButtonContainer);
            fishButton.Initialize(fishTypes[i], fishDisplay);

            if(i == 0)
            {
                fishButton.GetComponent<Button>().Select();
            }
        }
    }

    private void DisableFishButtons()
    {
        fishButtonContainer.gameObject.SetActive(false);
        fishDisplay.gameObject.SetActive(false);

        foreach(Transform child in fishButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }
}

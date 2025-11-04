using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BestiaryDisplayer : MonoBehaviour
{
    [Header("Prefabs References")]
    [SerializeField] private FishType[] fishTypes;
    [SerializeField] private FishButton fishButtonPrefab;

    [Header("Scene References")]
    [SerializeField] private Transform fishList;
    [SerializeField] private Transform fishButtonContainer;
    [SerializeField] private FishDisplay fishDisplay;

    private void Start()
    {
        InputManager.Instance.SelectButtonAction.performed += Enable;
    }

    private void Disable(InputAction.CallbackContext context)
    {
        if (fishList.gameObject.activeSelf && GameManager.Instance.State == GameManager.PlayerState.INMENU)
        {
            DisableFishButtons();
            GameManager.Instance.StopMenu();

            InputManager.Instance.SelectButtonAction.performed += Enable;
            InputManager.Instance.SelectButtonAction.performed -= Disable;
            InputManager.Instance.EastButtonAction.performed -= Disable;
        }
    }

    private void Enable(InputAction.CallbackContext context)
    {
        if (!fishList.gameObject.activeSelf && GameManager.Instance.State == GameManager.PlayerState.IDLE)
        {
            EnableFishButtons();
            GameManager.Instance.StartMenu();

            InputManager.Instance.SelectButtonAction.performed -= Enable;
            InputManager.Instance.SelectButtonAction.performed += Disable;
            InputManager.Instance.EastButtonAction.performed += Disable;
        }
    }

    private void EnableFishButtons()
    {
        fishList.gameObject.SetActive(true);
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
        fishList.gameObject.SetActive(false);
        fishDisplay.gameObject.SetActive(false);

        foreach(Transform child in fishButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }
}

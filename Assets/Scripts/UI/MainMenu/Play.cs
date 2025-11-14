using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] private Button button;

    private void InputModeChange(bool usingKeyboard) {
        if (!usingKeyboard)
            button.Select();
    }

    private void OnEnable()
    {
        InputManager.Instance.OnInputModeChangeEvent += InputModeChange;
        button.Select();
        button.onClick.AddListener(PlayGame);
    }

    private void OnDisable()
    {
        InputManager.Instance.OnInputModeChangeEvent -= InputModeChange;
    }

    private void PlayGame()
    {
        GameManager.Instance.CloseMainMenu(new InputAction.CallbackContext());
    }
}

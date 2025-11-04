using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    [SerializeField] private Button button;

    private void OnEnable()
    {
        button.Select();
        button.onClick.AddListener(PlayGame);
    }

    private void PlayGame()
    {
        GameManager.Instance.CloseMainMenu(new InputAction.CallbackContext());
    }
}

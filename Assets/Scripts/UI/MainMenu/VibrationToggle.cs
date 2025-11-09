using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VibrationToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    public void Awake()
    {
        toggle.toggleTransition = Toggle.ToggleTransition.None;
        toggle.isOn = GamepadVibration.Instance.CanVibrate;
        toggle.toggleTransition = Toggle.ToggleTransition.Fade;
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        GamepadVibration.Instance.EnableVibration(value);
    }
}

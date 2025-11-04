using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParticleToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    public void Awake() {
        toggle.toggleTransition = Toggle.ToggleTransition.None;
        toggle.isOn = FXManager.Instance.particlesEnabled;
        toggle.toggleTransition = Toggle.ToggleTransition.Fade;
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        FXManager.Instance.SetPaticlesEnabled(value);
    }
}

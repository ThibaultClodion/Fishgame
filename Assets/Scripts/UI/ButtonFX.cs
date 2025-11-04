using UnityEngine;
using UnityEngine.UI;

public class ButtonFX : MonoBehaviour
{
    [SerializeField] private AudioClip pressedClip;

    private void OnEnable()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(PlayPressedSound);
    }

    private void OnDisable()
    {
        Button button = GetComponent<Button>();

        button.onClick.RemoveListener(PlayPressedSound);
    }

    private void PlayPressedSound()
    {
        AudioManager.Instance.PlaySFX(pressedClip);
    }
}

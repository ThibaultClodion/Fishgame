using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DynamicKeyImage : MonoBehaviour
{
	private Image image;

	[SerializeField]
	private InputManager.KeyType type;
	public InputManager.KeyType Type {
		get {return type;}
		set {
			type = value;
			UpdateImage(InputManager.Instance.UsingKeyboard);
		}}

	public void SetAction(InputAction action) {
		// Will automatically update the image because of the setter
		Type = InputManager.Instance.KeyTypes[action];
	}

	private void Awake() {
		// Update image for the first time (potentially double update)
		UpdateImage(InputManager.Instance.UsingKeyboard);
	}

	private void OnEnable() {
		InputManager.Instance.OnInputModeChangeEvent += UpdateImage;
	}

	private void OnDisable() {
		InputManager.Instance.OnInputModeChangeEvent -= UpdateImage;
	}

	private void UpdateImage(bool usingKeyboard) {
		InputManager.KeyImage imgs = InputManager.Instance.KeyImages[type];
		if (image == null)
			image = GetComponent<Image>();
		image.sprite = usingKeyboard ? imgs.keyboard : imgs.gamepad;
	}
}

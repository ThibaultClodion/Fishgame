using UnityEngine;
using UnityEngine.UI;

public class QTEMiniGame : BaseMiniGame
{
	[Header("QTE Game Settings")]
	[SerializeField]
	private float buttonFailTime = 1.2f;
	[SerializeField]
	private float timeBetweenButtons = 3.0f;
	[SerializeField]
	private float progressionIncrease = 0.25f;
	[SerializeField]
	private float progressionDecrease = 0.1f;
	[SerializeField]
	private float progressionAutoDecreaseRate = 0.01f;

	[Header("Object References")]
	[SerializeField]
	private Slider timeSlider;
	[SerializeField]
	private DynamicKeyImage keyImage;

	// Index of currently selected random action to push
	private int currentButton;
	// Time at which the minigame will fail
	private float failTime;
	// Time at which the next button will be given
	private float nextButtonTime;

	public override void Initialize(FishData data)
	{
		base.Initialize(data);
		// Pick the next button 
		PickNextButton();
	}

	private void PickNextButton() {
		currentButton = Random.Range(0, InputManager.Instance.ButtonActions.Length-1);
		nextButtonTime = Time.time + timeBetweenButtons;
		failTime = Time.time + buttonFailTime;
		keyImage.SetAction(InputManager.Instance.ButtonActions[currentButton]);
		keyImage.gameObject.SetActive(true);
		timeSlider.gameObject.SetActive(true);
		timeSlider.value = 1.0f;
	}

	private void ClearButton() {
		currentButton = -1;
		keyImage.gameObject.SetActive(false);
		timeSlider.gameObject.SetActive(false);
	}

	// Update is called once per frame
	private void Update() {
		AddToProgression(-progressionAutoDecreaseRate * Time.deltaTime);

		if (Time.time >= nextButtonTime)
			PickNextButton();

		timeSlider.value = (failTime - Time.time) / buttonFailTime;

		if (currentButton == -1)
			return;

		bool pressedOtherButton = false;
		bool pressedRightButton = false;

		// Check all buttons and see if the user pressed the right button or another one
		for (int i=0;i<InputManager.Instance.ButtonActions.Length;i++) {
			if (!InputManager.Instance.ButtonActions[i].WasPressedThisFrame())
				continue;

			if (i == currentButton)
				pressedRightButton = true;
			else
				pressedOtherButton = true;
		}

		if (Time.time >= failTime || pressedOtherButton) {
			AddToProgression(-progressionDecrease);
			ClearButton();
		} else if (pressedRightButton) {
			AddToProgression(progressionIncrease);
			ClearButton();
		}
	}
}

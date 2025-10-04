using UnityEngine;
using UnityEngine.UI;

public class QTEMiniGame : BaseMiniGame
{
	[Header("QTE Time Settings")]
	[SerializeField]
	[Range(0.0f, 5.0f)]
	private float buttonFailTime = 1.2f;
	[SerializeField]
	[Range(0.0f, 5.0f)]
	private float timeBetweenButtons = 3.0f;

	[Header("QTE Time Randomness Settings")]
	[SerializeField]
	[Range(0.0f, 5.0f)]
	private float buttonFailTimeRandom = 0.2f;
	[SerializeField]
	[Range(0.0f, 5.0f)]
	private float timeBetweenButtonsRandom = 1.0f;

	[Header("QTE Progression Settings")]
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float progressionIncrease = 0.25f;
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float progressionDecrease = 0.1f;
	[SerializeField]
	[Range(0.0f, 1.0f)]
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
	// Duration of failTime for the slider
	private float failTimeDuration;
	// Time at which the next button will be given
	private float nextButtonTime;

	// Current fish-based difficulty scale for the button times
	private float difficulityScaleButtons;
	// Current fish-based difficulty scale for progress auto reduction
	private float difficulityScaleProgress;

	public override void Initialize(FishData data)
	{
		base.Initialize(data);

		difficulityScaleButtons = Mathf.Clamp((1.0f - data.Difficulty)*2.0f, 0.5f, 2.0f);
		difficulityScaleProgress = Mathf.Clamp(data.Difficulty*2.5f, 1.0f, 2.5f);

		// Pick the next button 
		PickNextButton();
	}

	private void PickNextButton() {
		currentButton = Random.Range(0, InputManager.Instance.ButtonActions.Length-1);

		nextButtonTime = Time.time + (timeBetweenButtons + Random.Range(-timeBetweenButtonsRandom, timeBetweenButtonsRandom)) * difficulityScaleButtons;
		failTimeDuration = (buttonFailTime + Random.Range(-buttonFailTimeRandom, buttonFailTimeRandom)) * difficulityScaleButtons;
		failTime = Time.time + failTimeDuration;

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
		AddToProgression(-progressionAutoDecreaseRate * difficulityScaleProgress * Time.deltaTime);

		if (Time.time >= nextButtonTime)
			PickNextButton();

		timeSlider.value = (failTime - Time.time) / failTimeDuration;

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

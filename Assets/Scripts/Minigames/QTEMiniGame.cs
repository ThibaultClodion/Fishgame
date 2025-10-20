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
	private float timeBetweenButtonsAfterPress = 1.5f;

	[Header("QTE Time Randomness Settings")]
	[SerializeField]
	[Range(0.0f, 5.0f)]
	private float buttonFailTimeRandom = 0.2f;
	[SerializeField]
	[Range(0.0f, 5.0f)]
	private float timeBetweenButtonsRandom = 0.8f;

	[Header("QTE Mashing Settings")]
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float buttonMashProbablity = 0.3f;
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float buttonMashDecrease = 0.2f;
	[SerializeField]
	[Range(0.0f, 1.0f)]
	private float buttonMashIncrease = 0.17f;

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
	[SerializeField]
	private GameObject mashSubgroup;
	[SerializeField]
	private Slider mashSlider;

	// Index of currently selected random action to push
	private int currentButton;
	// Is the current button a mashable button
	private bool mashButton;
	// Time at which the minigame will fail
	private float failTime;
	// Duration of failTime for the slider
	private float failTimeDuration;
	// Time at which the next button will be given
	private float nextButtonTime;
	// Duration of nextButton
	private float nextButtonDuration;

	// Current fish-based difficulty scale for the button times
	private float difficulityScaleButtons;
	// Current fish-based difficulty scale for progress auto reduction
	private float difficulityScaleProgress;

	// Random instance for QTE buttons
	private RealRandom random = new RealRandom(1);

	public override void Initialize(FishData data)
	{
		base.Initialize(data);

		difficulityScaleButtons = Mathf.Clamp((1.0f - data.Difficulty)*2.0f, 0.5f, 2.0f);
		difficulityScaleProgress = Mathf.Clamp(data.Difficulty*2.5f, 1.0f, 2.5f);

		// Pick the next button 
		PickNextButton();
	}

	private void PickNextButton() {
		currentButton = this.random.Range(0, InputManager.Instance.ButtonActions.Length-1);

		mashButton = Random.value <= buttonMashProbablity;

		nextButtonDuration = (timeBetweenButtonsAfterPress + Random.Range(-timeBetweenButtonsRandom, timeBetweenButtonsRandom)) * difficulityScaleButtons;
		failTimeDuration = (buttonFailTime + Random.Range(-buttonFailTimeRandom, buttonFailTimeRandom)) * difficulityScaleButtons * (mashButton ? 4.0f : 1.0f);
		failTime = Time.time + failTimeDuration;
		nextButtonTime = failTime + nextButtonDuration;

		keyImage.SetAction(InputManager.Instance.ButtonActions[currentButton]);
		keyImage.gameObject.SetActive(true);
		timeSlider.gameObject.SetActive(true);
		mashSubgroup.SetActive(mashButton);
		timeSlider.value = 1.0f;
		mashSlider.value = 0.0f;
	}

	private void ClearButton() {
		currentButton = -1;
		mashButton = false;
		keyImage.gameObject.SetActive(false);
		keyImage.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		timeSlider.gameObject.SetActive(false);
		mashSubgroup.SetActive(false);
	}

	private void ButtonOutcome(bool success) {
		AddToProgression(success ? progressionIncrease : -progressionDecrease);
		ClearButton();
		nextButtonTime = Mathf.Min(nextButtonTime, Time.time + nextButtonDuration);
	}

	// Update is called once per frame
	private void Update() {
		AddToProgression(-progressionAutoDecreaseRate * difficulityScaleProgress * Time.deltaTime);
		if (Time.time >= nextButtonTime)
			PickNextButton();

		if (currentButton == -1)
			return;

		timeSlider.value = (failTime - Time.time) / failTimeDuration;

		if (mashButton) {
			mashSlider.value -= buttonMashDecrease * difficulityScaleProgress * Time.deltaTime;
			float keyScale = (Mathf.PingPong((failTime - Time.time)*7.0f, 1.0f)*0.4f - 0.20f) + 1.0f;
			keyImage.transform.localScale = new Vector3(keyScale,keyScale,1.0f);
		}

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

		if (mashButton) {
			if (pressedRightButton)
				mashSlider.value += buttonMashIncrease;
			else if (pressedOtherButton)
				mashSlider.value -= buttonMashIncrease;

			if (mashSlider.value >= 1.0f) {
				ButtonOutcome(true);
			} else if (Time.time >= failTime) {
				ButtonOutcome(false);
			}
		} else {
			if (Time.time >= failTime || pressedOtherButton) {
				ButtonOutcome(false);
			} else if (pressedRightButton) {
				ButtonOutcome(true);
			}
		}
	}
}

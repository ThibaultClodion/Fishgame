using UnityEngine;
using UnityEngine.InputSystem;

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

	// Index of currently selected random action to push
	private int currentButton;
	// Time at which the minigame will fail
	private float failTime;
	// Time at which the next button will be given
	private float nextButtonTime;

	public override void Initialize()
	{
		base.Initialize();
		// Pick the next button 
		PickNextButton();
	}

	private void PickNextButton() {
		currentButton = Random.Range(0, InputManager.Instance.ButtonActions.Length-1);
		nextButtonTime = Time.time + timeBetweenButtons;
		failTime = Time.time + buttonFailTime;
		Debug.Log("Press " + InputManager.Instance.ButtonActions[currentButton]);
	}

	// Update is called once per frame
	private void Update() {
		GetOnAddToProgression()?.Invoke(-progressionAutoDecreaseRate * Time.deltaTime);

		if (Time.time >= nextButtonTime)
			PickNextButton();

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
			Debug.Log("Failed");
			GetOnAddToProgression()?.Invoke(-progressionDecrease);
			currentButton = -1;
		} else if (pressedRightButton) {
			Debug.Log("Nice");
			GetOnAddToProgression()?.Invoke(progressionIncrease);
			currentButton = -1;
		}
	}
}

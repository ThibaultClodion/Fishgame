using UnityEngine;

public class FXManager : MonoBehaviour
{
	public static FXManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		animationsEnabled = true;
		particlesEnabled = true;
	}

	public bool animationsEnabled { get; private set; }
	public bool particlesEnabled { get; private set; }

	public void SetAnimationEnabled(bool status) {
		animationsEnabled = status;
	}

	public void SetPaticlesEnabled(bool status) {
		particlesEnabled = status;
	}
}

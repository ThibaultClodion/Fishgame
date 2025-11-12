using UnityEngine;

[CreateAssetMenu(fileName = "FishBank", menuName = "Scriptable Objects/FishBank")]
public class FishBank : ScriptableObject
{
    [SerializeField]
	private string bankName;
	[SerializeField]
	private FishType[] fishTypes;

	// Random instance for the fish type
	private RealRandom random = new RealRandom(2);

	// Returns a random fish type
	public FishType getRandomType() {
		return fishTypes[this.random.Range(0, fishTypes.Length)];
	}
}

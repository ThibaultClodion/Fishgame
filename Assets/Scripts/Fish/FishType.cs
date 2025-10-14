using UnityEngine;

[CreateAssetMenu(fileName = "FishType", menuName = "Scriptable Objects/FishType")]
public class FishType : ScriptableObject
{
	[SerializeField]
	private string fishName;
	[SerializeField]
	private Vector2 difficulty;
	[SerializeField]
	private int excludedMinigames;
	[SerializeField]
	public Sprite sprite;
	[SerializeField]
	private Vector2 length;
	[SerializeField]
	private Vector2 width;
	// m/s
	[SerializeField]
	private Vector2 speed;

	public FishData GenerateFishData() {
		float diff = Random.Range(difficulty.x, difficulty.y);
		float len = Random.Range(length.x, length.y);
		float wdth = Random.Range(width.x, width.y);
		float spd = Random.Range(speed.x, speed.y);
		return new FishData(this, fishName, diff, excludedMinigames, sprite, len, wdth, spd);
	}
}

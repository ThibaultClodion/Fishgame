using UnityEngine;

[CreateAssetMenu(fileName = "FishType", menuName = "Scriptable Objects/FishType")]
public class FishType : ScriptableObject
{
	[SerializeField]
	private string Name;
	[SerializeField]
	private Vector2 Difficulty;
	[SerializeField]
	private int ExcludedMinigames;
	[SerializeField]
	private Sprite Sprite;
	[SerializeField]
	private Vector2 Longueur;
	[SerializeField]
	private Vector2 Largeur;

	public FishData GenerateFish() {
		float diff = Random.Range(Difficulty.x, Difficulty.y);
		float longueur = Random.Range(Longueur.x, Longueur.y);
		float larg = Random.Range(Largeur.x, Largeur.y);
		return new FishData(Name, diff, ExcludedMinigames, Sprite, longueur, larg);
	}
}

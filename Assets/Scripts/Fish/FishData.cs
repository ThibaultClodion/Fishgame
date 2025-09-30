using UnityEngine;

public class FishData
{
	public string Name {get; private set;}
	public float Difficulty {get; private set;}
	public int ExcludedMinigames {get; private set;}
	public Sprite Sprite {get; private set;}
	public float Longueur {get; private set;}
	public float Largeur {get; private set;}

	public FishData(string name, float diff, int excludedMG, Sprite spr, float longueur, float larg) {
		this.Name = name;
		this.Difficulty = diff;
		this.ExcludedMinigames = excludedMG;
		this.Sprite = spr;
		this.Longueur = longueur;
		this.Largeur = larg;
	}
}

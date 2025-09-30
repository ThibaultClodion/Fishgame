using UnityEngine;

public class FishData
{
	public string Name {get; private set;}
	public float Difficulty {get; private set;}
	public int ExcludedMinigames {get; private set;}
	public Sprite Sprite {get; private set;}
	public float Length {get; private set;}
	public float Width {get; private set;}
	// m/s
	public float Speed {get; private set;}

	public FishData(string name, float diff, int excludedMG, Sprite spr, float len, float width, float spd) {
		this.Name = name;
		this.Difficulty = diff;
		this.ExcludedMinigames = excludedMG;
		this.Sprite = spr;
		this.Length = len;
		this.Width = width;
		this.Speed = spd;
	}
}

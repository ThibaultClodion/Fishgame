using UnityEngine;

public class FishData
{
	public FishType Type {get; private set;}
	public string Name {get; private set;}
	// Number 0 - 1, 0 is very easy, 1 is very hard
	public float Difficulty {get; private set;}
	public int ExcludedMinigames {get; private set;}
	public Sprite Sprite {get; private set;}
	public float Length {get; private set;}
	public float Width {get; private set;}
	// m/s
	public float Speed {get; private set;}

	public FishData(FishType type, string name, float diff, int excludedMG, Sprite spr, float len, float width, float spd) {
		this.Type = type;
		this.Name = name;
		this.Difficulty = diff;
		this.ExcludedMinigames = excludedMG;
		this.Sprite = spr;
		this.Length = len;
		this.Width = width;
		this.Speed = spd;
	}
}

using UnityEngine;

// Sprite Renderer needed to define zone
[RequireComponent(typeof(BoxCollider2D))]
public class FishSpawner : MonoBehaviour
{
	[SerializeField]
	[Tooltip("All the possible fish type spawns")]
	private FishType[] fishTypes;

	[Header("Spawned Fish Settings")]
	[SerializeField]
	[Tooltip("The fish prefab (requires Fish component)")]
	private GameObject fishPrefab;

	[SerializeField]
	[Tooltip("The direction in which fish will swim")]
	private Vector2 fishSwimDirection;

	[Header("Spawn Times")]
	[SerializeField]
	[Tooltip("Time between spawn in ms")]
	[Range(0,2000)]
	private float spawnTime = 750;

	[SerializeField]
	[Tooltip("+/- random time in ms added to the spawn time")]
	[Range(0,1000)]
	private float spawnTimeRandom = 250;

	// Reference to the collider for the shape
	private BoxCollider2D boxCollider;

	// Stores the next spawn time
	private float nextSpawnTime;

	void Spawn() {
        // Sanity Check
        if (fishPrefab == null) {
			Debug.LogError("No Fish Prefab for Spawner");
			nextSpawnTime = float.PositiveInfinity;
			return;
		}

		// Pick random point inside
		Vector2 point = new Vector2(Random.Range(transform.position.x - boxCollider.size.x/2.0f, transform.position.x + boxCollider.size.x/2.0f), 
									Random.Range(transform.position.y - boxCollider.size.y/2.0f, transform.position.y + boxCollider.size.y/2.0f))
						+ boxCollider.offset;

		// Pick a random FishType
		FishType type = fishTypes[Random.Range(0, fishTypes.Length)];

		// Create a FishData from it
		FishData data = type.GenerateFishData();

		// Instantiate a Fish Object
		// TODO: Instantiate Fish Obj in another parent object ?
		GameObject fishObj = Instantiate(fishPrefab, point, Quaternion.identity, transform);

		// Initialize the Fish Component
		Fish fish = fishObj.GetComponent<Fish>();
		if (fish == null) {
			Debug.LogError("Spawned Fish doesn't have fish Component");
			nextSpawnTime = float.PositiveInfinity;
			return;
		}
		fish.Init(data, fishSwimDirection);

		// Set next spawn time
		nextSpawnTime = Time.time + (spawnTime + Random.Range(-spawnTimeRandom, spawnTimeRandom)) / 1000.0f;
	}

	void Awake() {
		this.boxCollider = GetComponent<BoxCollider2D>();
	}

	void Update() {
		if (Time.time >= nextSpawnTime)
			Spawn();
	}
}

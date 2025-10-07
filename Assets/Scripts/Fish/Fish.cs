using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Fish : MonoBehaviour
{
	// Our data instance
	private FishData data;

	// Component references
	private SpriteRenderer spriteRenderer;
	private Rigidbody2D rigid;

	// Called from FishSpawner
	public void Init(FishData dat, Vector2 direction) {
		// Bind data
		this.data = dat;
		// Get Components
		this.spriteRenderer = GetComponent<SpriteRenderer>();
		this.rigid = GetComponent<Rigidbody2D>();
		
		// Apply fish data size
		transform.localScale = new Vector3(this.data.Length, this.data.Width, 1.0f);
		// Apply fish direction rotation
		float angle = Vector3.Angle(Vector3.right, direction);
		transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
		// Flip sprite if we are facing the other way
		this.spriteRenderer.flipY = Mathf.Abs(angle) >= 90 && Mathf.Abs(angle) <= 270;
		//this.spriteRenderer.sprite = dat.Sprite;

		//gameObject.name = data.Name + Hash128.Compute((Time.time + Random.Range(0.0f, 1.0f)).ToString()).ToString();
	}

	// Destroys itself and returns data
	public FishData Catch() {
		// Queue for destruction
		Destroy(gameObject);
		// Return data
		return this.data;
	}

	void Update() {
		// Use the rigidbody to handle movement
		this.rigid.linearVelocity = transform.right * data.Speed;

		// TODO: Replace this with a better check
		if (Mathf.Abs(transform.position.x) >= 10.0f + data.Length/2.0f)
			Destroy(gameObject);
	}
}

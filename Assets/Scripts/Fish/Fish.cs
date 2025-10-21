using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Fish : MonoBehaviour
{
	// Our data instance
	private FishData data;

	enum FishState {
		SWIMING,
		HOOKED,
		CAUGHT
	};

	// Current fish state
	private FishState state;

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

		this.state = FishState.SWIMING;

		// Apply fish data size
		transform.localScale = new Vector3(this.data.Length, this.data.Width, 1.0f);
		// Apply fish direction rotation
		float angle = Vector3.Angle(Vector3.right, direction);
		transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);
		// Flip sprite if we are facing the other way
		this.spriteRenderer.flipY = Mathf.Abs(angle) >= 90 && Mathf.Abs(angle) <= 270;
		this.spriteRenderer.sprite = dat.Sprite;

		//gameObject.name = data.Name + Hash128.Compute((Time.time + Random.Range(0.0f, 1.0f)).ToString()).ToString();
	}

	public FishData Hook() {
		if (this.state != FishState.SWIMING) {
			Debug.LogError("Trying to hook while not swiming");
			return null;
		}
		this.state = FishState.HOOKED;

		// Return data
		return this.data;
	}

	public void UnHook() {
		if (this.state != FishState.HOOKED) {
			Debug.LogError("Trying to unhook while not hooked");
			return;
		}
		this.state = FishState.SWIMING;
	}

	// Destroys itself and returns data
	public FishData Catch(Vector3 moveToPosition) {
		if (this.state != FishState.HOOKED) {
			Debug.LogError("Trying to catch while not hooked");
			return null;
		}

		this.state = FishState.CAUGHT;

		// Return data
		return this.data;
	}

	// Destroys itself
	public void Finish() {
		if (this.state != FishState.CAUGHT) {
			Debug.LogError("Trying to finish while not caught");
			return;
		}

		// Queue for destruction
		Destroy(gameObject);
	}

	void Update() {
		// TODO: Replace this with a better check
		if (Mathf.Abs(transform.position.x) >= 10.0f + data.Length/2.0f)
			Destroy(gameObject);

		// TODO: Struggle animation
		if (this.state == FishState.HOOKED) {

		}

		// Only move if we're not hooked
		// Use the rigidbody to handle movement
		this.rigid.linearVelocity = this.state == FishState.SWIMING ? transform.right * data.Speed : Vector3.zero;
	}
}

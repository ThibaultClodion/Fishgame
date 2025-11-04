using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class Fish : MonoBehaviour
{
	// Our data instance
	public FishData Data { get; private set; }

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
	private Collider2D colliderRef;
	private ParticleSystem particles;

	// Catch animation parameters
	private Vector3 positionWhenHooked;
	private Vector3 moveToPosition;
	private float outWaterTime;
	private float timeBeforeDestroy;
	private float caughtTime;

	// Called from FishSpawner
	public void Init(FishData dat, Vector2 direction) {
		// Bind data
		this.Data = dat;
		// Get Components
		this.spriteRenderer = GetComponent<SpriteRenderer>();
		this.rigid = GetComponent<Rigidbody2D>();
		this.colliderRef = GetComponent<Collider2D>();
		this.particles = GetComponent<ParticleSystem>();

		this.state = FishState.SWIMING;

		// Apply fish data size
		transform.localScale = new Vector3(this.Data.Length, this.Data.Width, 1.0f);
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
		this.positionWhenHooked = transform.position;

		// Return data
		return this.Data;
	}

	public void UnHook() {
		if (this.state != FishState.HOOKED) {
			Debug.LogError("Trying to unhook while not hooked");
			return;
		}
		this.state = FishState.SWIMING;
	}

	// Destroys itself and returns data
	public FishData Catch(Vector3 moveToPosition, float outWaterTime, float beforeDestroyTime) {
		if (this.state != FishState.HOOKED) {
			Debug.LogError("Trying to catch while not hooked");
			return null;
		}

		GamepadVibration.Instance.Vibration(0f, 1f, 0.3f);

		this.caughtTime = Time.time;
		this.moveToPosition = moveToPosition;
		this.outWaterTime = outWaterTime;
		this.timeBeforeDestroy = beforeDestroyTime;

		this.state = FishState.CAUGHT;

		// Return data
		return this.Data;
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
		if (Mathf.Abs(transform.position.x) >= 10.0f + Data.Length/2.0f)
			Destroy(gameObject);

		// TODO: Struggle animation
		if (this.state == FishState.HOOKED) {
			transform.position = this.positionWhenHooked + new Vector3(Mathf.Sin(Time.time * 9.0f) * 0.1f, 
																	   Mathf.Sin(Mathf.Sin((Time.time + 1.6f) * 4.0f) * 3.14f * 2.0f) * 0.05f, 0.0f);
		}

		// TODO : stuck player during catch animation
		if (this.state == FishState.CAUGHT) {
			//Move to moveToPosition in outWaterTime seconds
			float t = (Time.time - caughtTime) / outWaterTime;
			transform.position = Vector3.Lerp(positionWhenHooked, moveToPosition, t);

			if (caughtTime + outWaterTime + timeBeforeDestroy <= Time.time) {
				Finish();
			}
		}

		// Change emission depending on state
		var particleEmission = this.particles.emission;
		particleEmission.rateOverTime = Data.Speed * (this.state == FishState.SWIMING ? 1.0f : (this.state == FishState.HOOKED ? 10.0f : 0.0f));

		// Only colide with harpoon when swiming
		this.colliderRef.enabled = this.state == FishState.SWIMING;

		// Only move if we're swiming
		// Use the rigidbody to handle movement
		this.rigid.linearVelocity = this.state == FishState.SWIMING ? transform.right * Data.Speed : Vector3.zero;
	}
}

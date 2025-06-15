using UnityEngine;

public class PlayerController : MonoBehaviour{

	public float baseSpeed = 2f;
	[SerializeField] private float swimForce = 4f;
	[HideInInspector] public float currentSpeed;
	
	private float speedMultiplier = 1f;
	private float trashDebuff = 0f;

	private Rigidbody2D rb;

	void Awake()
	{
		currentSpeed = baseSpeed;
	}
	
	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();	
	}
	
	void Update()
	{
		HandleInput ();
	}
	
	private void HandleInput()
	{
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		Vector2 input = new Vector2(horizontal, vertical);

		// Normalize input for consistent direction
		if (input.sqrMagnitude > 1)
			input = input.normalized;

		// Apply force to simulate swimming
		rb.AddForce(input * swimForce);

		// Limit max swim speed
		if (rb.linearVelocity.magnitude > currentSpeed)
		{
			rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
		}

		// Flip sprite when swimming left/right
		if (horizontal > 0)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0f, transform.rotation.eulerAngles.z);
		}
		else if (horizontal < 0)
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 180f, transform.rotation.eulerAngles.z);
		}
		
		// Rotate player on W/S press
		if (Input.GetKeyDown(KeyCode.W))
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 45f);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, -45f);
		}

		if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
		}
	}

	public void ApplySpeedDebuff(float debuff)
	{
		trashDebuff += debuff;
		UpdateSpeed();
	}

	public void ResetDebuffOnly()
	{
		trashDebuff = 0f;
		UpdateSpeed();
	}

	public void ResetToolEffects()
	{
		speedMultiplier = 1f;
		UpdateSpeed();
	}

	public void ResetAllSpeedModifiers()
	{
		trashDebuff = 0f;
		speedMultiplier = 1f;
		UpdateSpeed();
	}

	public void SetSpeedMultiplier(float multiplier)
	{
		speedMultiplier = multiplier;
		UpdateSpeed();
	}

	private void UpdateSpeed()
	{
		currentSpeed = Mathf.Max(0.1f, baseSpeed * speedMultiplier - trashDebuff);
	}
}
